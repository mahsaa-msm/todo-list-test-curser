| Field | Value |
| --- | --- |
| **name** | `/git-pull` |
| **id** | `git-pull` |
| **category** | Git |
| **file** | `git-pull.md` |
| **description** | Pull latest commits from `origin` into the current branch (upstream or explicit ref). Stop on conflicts. |

Authoring rules: `.cursor/rules/cursor-slash-commands.mdc`.

# Pull current branch from remote

Run **`git pull`** for this workspace **without asking the user to confirm** each step. Only stop and ask if the situation is **blocked** (no `origin`, auth failure) or **manual resolution is required** (merge/rebase conflicts).

## Before you start

- Prefer the configured upstream: plain `git pull` when the branch tracks `origin/<same-name>`.
- If **no upstream** is set, use `git pull origin <current-branch>` after reading the branch with `git branch --show-current`.
- Default merge strategy is Git’s default for `git pull` (typically **merge** unless the repo/user configured **rebase**). Do **not** switch global pull strategy in this command.
- Follow the same professionalism as `.cursor/commands/git-commit.md` (clear status, short summary).

## Steps (execute in order)

1. `git status -sb` — if the working tree is dirty, mention it in one line (pull may still succeed or may conflict on touched files).
2. `git remote -v` — if `origin` is missing, stop and say the remote URL must be configured.
3. `git rev-parse --abbrev-ref HEAD` to get the current branch name.
4. If upstream exists (`git rev-parse --abbrev-ref --symbolic-full-name @{u}` succeeds), run **`git pull`**. Otherwise run **`git pull origin <current-branch>`**.
5. If the result is **already up to date**, say so briefly.
6. If **conflicts** are reported, **stop**; list conflicted paths and tell the user to resolve, then continue with **`/git-commit`** / normal Git workflow—do not guess resolutions.

## Do not

- Do **not** run `git pull --rebase` unless the user explicitly asked for rebase in the same turn (keeps behavior predictable and matches default `git pull`).
- Do **not** `git fetch` + destructive reset, **force-pull**, or rewrite history unless the user explicitly asked.
- This command is **pull only**—no commit, no push.

## Related

- **`/git-commit`** — after resolving conflicts from a pull.
- **`/git-push`** — publish local commits; consider **`/git-pull`** first if you may be behind `origin`.
