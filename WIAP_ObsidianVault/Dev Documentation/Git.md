# .gitattributes
The .obsidian/** has been added to avoid obsidian rewriting local `cr lf` endings to `lf`, which causes git to display the config files as changes, even when nothing changed in the file itself. (This probably only happens when autocrlf is enabled in global git config locally on a windows machine.)

According to [this documentation](https://docs.github.com/en/get-started/getting-started-with-git/configuring-git-to-handle-line-endings) you can set repository wide overrides of the core.autocrlf setting of git with the .gitattributes.  The actuall setting to disable autocrlf repo wide is described [in this SO answer](https://stackoverflow.com/a/52996849).

# .gitignore
## Unity
The unity gitignore part was compiled from sources such as [this](https://github.com/github/gitignore/blob/main/Unity.gitignore) and the default unity .gitignore.

## Obsidian
After working on the project for a bit we realized the need to exclude obsidians 'workspace' file in the .obsidian folder. It only contains local user information and is not needed (as written [here](https://github.com/trustedsec/Obsidian-Vault-Structure/blob/main/.gitignore)).