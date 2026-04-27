| Field | Value |
| --- | --- |
| **name** | `/git-branch` |
| **id** | `git-branch` |
| **category** | Git |
| **file** | `git-branch.md` |
| **description** | Create and switch to a new local branch from HEAD using `git switch -c` (name from the user message). |

Authoring rules: `.cursor/rules/cursor-slash-commands.mdc`.

# Create and switch to a new branch

Create a **new local branch** from the current **`HEAD`** and switch to it using **`git switch -c`**. Run steps **without asking for confirmation** once the branch name is known.

## Branch name (required)

- Read the **same chat message** that invoked **`/git-branch`** for a branch name (examples: `feat/todo-api`, `fix/auth-null`, `chore/update-deps`).
- Accept **kebab-case** / **slash segments** common in Git (`feat/foo`, `issue-12-login`).
- If **no usable name** appears in that message, **stop** and reply with a one-line usage example: **`/git-branch feat/todo-crud`** (do not invent a name).

## Before you start

- Follow the same professionalism as `.cursor/commands/git-commit.md` (clear status, short summary).
- If a branch with that name **already exists** locally, **stop** and report the conflict—do not overwrite.
- If **`origin`** is configured, check the remote with **`git ls-remote --heads origin <name>`** — if output is non-empty, **stop** (branch already exists on the remote) unless the user explicitly asked to reuse it.

## Steps (execute in order)

1. `git status -sb` and note the current branch.
2. Extract the **branch name** from the user message (trim whitespace; reject empty).
3. `git show-ref --verify --quiet refs/heads/<name>` — if exit code `0`, stop: branch already exists locally.
4. If `git remote get-url origin` succeeds, run **`git ls-remote --heads origin <name>`** — if there is any output line, stop: branch exists on **`origin`**.
5. Run **`git switch -c <name>`** (creates from current `HEAD` and checks out).
6. Show `git status -sb` and `git branch --show-current` to confirm.

## Do not

- Do **not** delete, rename, or **force**-move branches unless the user explicitly asked for that in the same turn.
- Do **not** `git push` or set upstream here—use **`/git-push`** with **`-u`** when the user is ready to publish (they can ask in a follow-up).
- Do **not** create a branch from an arbitrary old commit unless the user explicitly gave a **base ref** in the same message—default base is **current `HEAD`**.

## Related

- **`/git-commit`** — commit on the new branch.
- **`/git-pull`** — sync **`main`** (or base branch) before branching if you started from an outdated tip.
- **`/git-push`** — first publish of the new branch typically needs **`git push -u origin <name>`** (handled when the user runs **`/git-push`** and upstream is missing, same pattern as other commands).
