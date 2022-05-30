## EOL fix
During programming and specifically when using obsidian a number of files changed line endings back and forth between CRLF and LF.
With `* -text` the global (local) git config for autocrlf is overridden to avoid dirty commits. 
At the same time `.obsidian/** eol=lf` tries to enforce the LF line ending obsidian changes to after regenrating the config files.

Some strange behaviour is still happening, but this seemingly fixed the main problem.

## Linguist fixes
Linguist refers to the programm used to identify language used in a git repository. To keep that statistic clean from the Obsidian files we modify the gitattributes to include the tags described [here](https://github.com/github/linguist/blob/master/docs/overrides.md).