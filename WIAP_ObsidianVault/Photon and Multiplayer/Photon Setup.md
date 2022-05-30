# Setup for Photon
(Following [this Tutorial](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/tutorials/mr-learning-sharing-01))
## Step 1: Getting Unityhub/Unity ready 
[[IDE Setup]]
## Step 2: Download Photon
[Official site for Photon and its download](https://www.photonengine.com/pun)
![](attachments/Pasted%20image%2020220514190820.png)
Login when needed. On first try, the blue box will show "Download".
![](attachments/Pasted%20image%2020220514190935.png)
After download, it will lead you to Unity. Open your project and then "Download"+"Import" to make sure you have the newest version.
![](attachments/Pasted%20image%2020220514191659.png)
After those steps, photon is ready for use and implemented as Asset.
![](attachments/Pasted%20image%2020220514191759.png)

## Step 3: Creating a Photon User
Login or Signup at [Multiplayer Game Development Made Easy | Photon Engine](https://id.photonengine.com/)

## Step 4: Create Photon app
Photon Dashboard -> Create new Application:
![](attachments/Pasted%20image%2020220515170843.png)

After that copy the "App ID" und use that in the PUN setup Wizard to connect to the Project to the Photon Application.


## Step 5: Follow Tutorial
From [unity3d - MRTK Photon Unity Networking 'AnchorModuleScript' namespace not found - Stack Overflow](https://stackoverflow.com/questions/71942925/mrtk-photon-unity-networking-anchormodulescript-namespace-not-found): Keep in mind to install [Release Azure Spatial Anchors v2.7.2 · microsoft/MixedRealityLearning (github.com)](https://github.com/microsoft/MixedRealityLearning/releases/tag/azure-spatial-anchors-v2.7.2) since it is required to run the Tutorial

This also requires the ![](attachments/Pasted%20image%2020220515172838.png)
Azure Spatial Anchors SDK to be installed