# Commit staged and unstaged changes

Run a full **local `git commit` workflow** for this workspace **without asking the user to confirm** each step. Only stop and ask if there is **nothing to commit** or the changes are **ambiguous** (unrelated work mixed in one diff).

**Scope:** this command **never** touches the remote. It **must not** run `git push`, even if the user’s message is long or mentions publishing—**push is only when they run `/push` themselves.**

## Before you start

- Follow project commit rules in `.cursor/rules/git-commit-rule.mdc` (**Conventional Commits**).
- Prefer **one logical commit** per invocation; if the working tree clearly mixes unrelated changes, split into two commits **without** waiting for the user—use your judgment from `git diff`.

## Steps (execute in order)

1. `git status -sb` and `git diff` (and `git diff --staged` if anything is staged).
2. If the working tree is clean, reply briefly that there is nothing to commit and stop.
3. Stage what belongs together: `git add -A` **or** narrower paths if that avoids mixing unrelated edits.
4. Write the commit message:
   - Format: (see allowed types in `git-commit-rule.mdc`).
   - Summary: imperative, no trailing period, max ~72 chars for the first line.
5. Create the commit:
   - **Windows PowerShell**: do **not** use `git commit -m "type(scope): ..."` with **double quotes** if the message contains `(scope)`—PowerShell treats `(scope)` as a subexpression. Use either:
     - `git commit -F <tempfile>` where the file is **UTF-8 without BOM** and contains the full message (subject + blank line + body if needed), or
     - `git commit -m 'type(scope): summary'` using **single-quoted** `-m` when safe.
   - On other shells, `git commit -m` with a normal quoted string is fine.
6. Show the result: `git log -1 --oneline` and a one-line summary of what was committed.

## Do not

- Do **not** amend or force-push unless the user explicitly asked for that in the same turn.
- Do **not** run `git push` (or any remote update: `git pull`, `git fetch` for publishing). **Stop after commit**; do not ask “should I push?” unless the user explicitly asked about push in that message.
- The user publishes manually with **`/push`** when they want—**never** combine push into this flow.

## Related

- **`/push`** — separate command; only run when the user invokes **`/push`**, not after **`/commit`**.
