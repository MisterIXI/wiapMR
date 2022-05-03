- Unityhub 3.1.1 install 2021.3.0f1 (lts)
![](attachments/Pasted%20image%2020220419224200.png)
	- Android Build Support (Android SDK; OpenJDK)
	- Universal Windows Platform Build Support
	- Windows Build Support (IL2CPP)

- Install VS 2019
![](attachments/Pasted%20image%2020220419232028.png)
	- Desktop dev with C++
	- Mobile dev with C++
	- Universal Windows Platform developement
		- "C++ (v142)"
		- "C++ (v141)"

- [Setting up your XR configuration - Mixed Reality | Microsoft Docs](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/xr-project-setup?tabs=openxr)
- [Introduction to the Mixed Reality Toolkit--Set Up Your Project and Use Hand Interaction - Learn | Microsoft Docs](https://docs.microsoft.com/en-us/learn/modules/learn-mrtk-tutorials/1-1-introduction?tabs=openxr&ns-enrollment-type=learningpath&ns-enrollment-id=learn.azure.beginner-hololens-2-tutorials)
- Download this: [Download Mixed Reality Feature Tool from Official Microsoft Download Center](https://www.microsoft.com/en-us/download/details.aspx?id=102778)
# connect Hololens with pc
- create a blank uwp app in visual studio
   ![](attachments/Pasted%20image%2020220419235618.png)
   - Hololens settings -> security -> connect for pin
   - select "remote machine" -> Hololens
      ![](attachments/Pasted%20image%2020220419235905.png)
  - Enter Pin when asked
  - UWP should now open after build

# unity project
![](attachments/Pasted%20image%2020220420000209.png)
Create 3D project
- use the "Microsoft Mixed Reality Feature Tool" to add the following two features:
 ![](attachments/Pasted%20image%2020220420000735.png)
	 - MRTK Foundation
	 - Mixed Reality OpenXR Plugin

# Unity config
In Project Configurator:
- Unity OpenXR Plugin
- in right most tab activate
- ![](attachments/Pasted%20image%2020220420001205.png)
- ![](attachments/Pasted%20image%2020220420001240.png)
## build and run settings
![](attachments/Pasted%20image%2020220420002022.png)
- Portal adress (ip changes; check wifi properties for IP of Hololens in current network)
- Username: mengel-th-bingen
