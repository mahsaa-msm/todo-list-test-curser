| Field | Value |
| --- | --- |
| **name** | `/git-push` |
| **id** | `git-push` |
| **category** | Git |
| **file** | `git-push.md` |
| **description** | Push the current branch to `origin` (set upstream if missing). Never force-push unless explicitly requested. |

**Filename:** use the **lowercase category slug** as prefix — for Git, `git-` — then the command id: `git-<id>.md`.

# Push current branch to remote

Run **`git push`** for this workspace **without asking the user to confirm** each step. Only stop and ask if the situation is **blocked or unsafe** (no `origin`, diverged history needing merge/rebase, auth failure you cannot fix in-tool).

## Before you start

- Prefer the tracked upstream: `git push` when `branch...origin/branch` is already configured.
- If there is **no upstream** yet, set it once: `git push -u origin <current-branch>` (discover branch with `git branch --show-current`).
- Follow the same professionalism as `.cursor/commands/git-commit.md` (clear status, short summary).

## Steps (execute in order)

1. `git status -sb` (note if working tree is dirty—still allowed to push; mention it in one line).
2. `git remote -v` — if `origin` is missing, stop and say the remote URL must be configured.
3. `git push` (or `git push -u origin <branch>` when upstream is missing).
4. If push reports **up to date**, say so briefly.
5. Show the remote line from output (e.g. `main -> main`) or error summary if it failed.

## Do not

- Do **not** `git push --force` or `--force-with-lease` unless the user explicitly asked for a force push in the same turn.
- Do **not** amend, rebase, or rewrite history unless the user explicitly asked—this command is **push only**.

## Related

- Use **`/git-commit`** to create commits first; use **`/git-push`** to publish them.
