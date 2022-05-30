### Intellisense not working
According to [this issue](https://github.com/OmniSharp/omnisharp-vscode/issues/5074#issuecomment-1049459608) you see that you have to change the settings of the C# extension "Omnisharp" in vscode to *not* use the build for .net 6 since that doesn't work for Unity.
In vscode go to settings, search for "Omnisharp: Use Modern Net" and **disable** that setting:
![](attachments/Pasted%20image%2020220529202757.png)
