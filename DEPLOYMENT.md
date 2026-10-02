# Docker publishing and automatic updates

The application supports three workflows:

| Workflow | Command | Database |
| --- | --- | --- |
| VS Code | Existing API + Client launch configuration | Windows LocalDB |
| Local Docker builds | `docker compose up -d --build` | Docker SQL Server |
| Published Docker images | `docker compose -f compose.deploy.yaml up -d` | Same Docker SQL Server volumes on this host |

Use the local and deployment Compose files separately. They manage the same
`nookly` stack and port; do not start them under different project names concurrently.

## 1. Publish the first release

Commit the Dockerfiles, Nginx configuration, Compose files, workflow, and all
application/migration changes needed for the working Docker version. `.env` is
ignored and must remain on the deployment machine.

Merge or push the changes to `main` or `master`. Your current development branch
is `Docker`: pushing that branch alone will not publish a release. Open a pull
request into `main` or `master` to run the checks, then merge it to publish.

On GitHub, open **Actions → Build, check, and publish containers**.
The workflow builds both images, starts a disposable SQL Server database,
checks migrations and API/client routing, and recreates the API to check proxy
reconnection. Only successful main/master builds publish to GHCR:

- `ghcr.io/morina8700/nookly-api:latest`
- `ghcr.io/morina8700/nookly-client:latest`
- Both images also have `sha-<full-commit-SHA>` tags.

GitHub Actions uses its built-in `GITHUB_TOKEN`; no SQL password, JWT key, or
personal GitHub token needs to be added to repository secrets. If publishing is
denied, check that GitHub Actions is allowed for the repository and the packages
grant this repository Actions access. The workflow requests `packages: write`.

## 2. Configure registry access on the Docker machine

Keep your existing `.env`: replacing the SQL password will not change the
password in the initialized SQL Server volume.

Create a GitHub **personal access token (classic)** with `read:packages` and
access to the published packages. Append these values to your existing `.env`:

```dotenv
GHCR_USERNAME=Morina8700
GHCR_TOKEN='YOUR_READ_PACKAGES_TOKEN'
```

The images can stay private even though the source repository is public.
Watchtower reads these credentials from `.env` through Compose. Anyone with
access to the Docker daemon can inspect container environment variables.

Sign in from PowerShell so Compose can pull the private images. Enter the same
token at the hidden password prompt:

```powershell
$registryCredential = Get-Credential -UserName Morina8700 -Message 'Enter your GHCR read:packages token as the password'
$registryCredential.GetNetworkCredential().Password | docker login ghcr.io -u $registryCredential.UserName --password-stdin
Remove-Variable registryCredential
```

Renew the token and update `.env` and Docker login before it expires.

## 3. Switch the existing stack to published images

Wait for the first GitHub publishing run to succeed, then run from the repository root:

```powershell
docker compose -f compose.deploy.yaml config --quiet
docker compose -f compose.deploy.yaml pull
docker compose -f compose.deploy.yaml up -d
docker compose -f compose.deploy.yaml ps
docker compose -f compose.deploy.yaml logs --tail=100 api watchtower
```

The deployment file explicitly uses project name `nookly`, preserving the
existing `nookly_db-data` and `nookly_uploads-data` volumes on this machine.
No `down` or volume removal is necessary. A different machine starts with new
volumes; transfer SQL backups and uploads if it needs the existing data.

Open `http://127.0.0.1:8080` and verify your Docker login and properties still work.
The app remains accessible only on this machine, matching the existing setup.
School LAN access requires a deliberate port/firewall/HTTPS configuration change.

## 4. Confirm automatic updates

Push or merge a small visible change into `main` or `master`. After Actions
publishes the images, Watchtower checks GHCR every 300 seconds and recreates
the opted-in API/client containers. Check:

```powershell
docker compose -f compose.deploy.yaml logs --tail=100 watchtower api
```

API startup applies outstanding EF migrations. Nginx uses Docker DNS to refresh
the API address when the API is replaced. Brief downtime while migrations run
or containers restart is expected. The API and client tags are published
separately; keep changes compatible across a brief version mismatch.

The client is a PWA: close all app tabs and reopen them if an old UI remains cached.
Watchtower has Docker socket access and is limited by labels and scope to this
deployment's API and client. SQL Server and Watchtower itself are not opted in.
The updater uses the maintained `nickfedor/watchtower` fork.

Watchtower updates images, not Compose settings. Reapply the deployment Compose
file when changing environment variables, ports, volumes, or labels. Database
volumes persist, but they are not backups: back up SQL data and uploads before
schema changes. Image rollback does not reverse database migrations.

## 5. Return to local development

VS Code development still uses LocalDB and does not require stopping Docker.
To return the Docker stack itself to locally built images, stop the updater first:

```powershell
docker compose -f compose.deploy.yaml stop watchtower
docker compose up -d --build --remove-orphans
```

This retains the database and upload volumes. Do not use `down -v` for this stack.

## References

- [GitHub: publishing Docker images](https://docs.github.com/en/actions/tutorials/publish-packages/publish-docker-images)
- [GitHub: registry authentication and package access](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)
- [Watchtower fork and documentation](https://github.com/nicholas-fedor/watchtower)
- [Watchtower: registry credentials](https://watchtower.nickfedor.com/v1.22.3/getting-started/usage/)
