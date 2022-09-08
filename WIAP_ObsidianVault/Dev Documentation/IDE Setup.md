# Install Unity
First download Unityhub: [Download - Unity (unity3d.com)](https://unity3d.com/get-unity/download)

In Unityhub install 2021.3.0f1 (lts) (later is fine, but it will convert the project)
![](attachments/Pasted%20image%2020220419224200.png)
	- Android Build Support (Android SDK; OpenJDK)
	- Universal Windows Platform Build Support
	- Windows Build Support (IL2CPP)

# Install Visual Studio 2019 (required for Hololens)
Install Visual studio from here: [Download Visual Studio Tools - Install Free for Windows, Mac, Linux (microsoft.com)](https://visualstudio.microsoft.com/downloads/)

In the installer install VS 2019:
![](attachments/Pasted%20image%2020220419232028.png)
	- Desktop dev with C++
	- Mobile dev with C++
	- Universal Windows Platform developement
		- "C++ (v142)"
		- "C++ (v141)"


# Hololens Device
In order to be able to deploy to the Hololens 2 you need to connect your PC to the device portal of the hololens.

For this the Hololens needs to be on the same network as your PC.
Related Tutorial: [Using Visual Studio to deploy and debug - Mixed Reality | Microsoft Docs](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2)
In this Tutorial you only need the first few steps. You can use [this video](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2#deploying-a-hololens-app-over-wi-fi-or-usb) as a guide 

The following steps need to be done:

### Developer mode on HoloLens

1.  Turn on your HoloLens and put on the device.
2.  Use the [start gesture](https://docs.microsoft.com/en-us/windows/mixed-reality/design/system-gesture) to launch the main menu.
3.  Select the **Settings** tile to launch the app in your environment.
4.  Select the **Update** menu item.
5.  Select the **For developers** menu item.
6.  Enable **Use developer features** to deploy apps from Visual Studio to your HoloLens. If your device is running Windows Holographic version 21H1 or newer, also enable **Device discovery**.
7.  Optional: Scroll down and also enable **Device Portal**, which lets you connect to the [Windows Device Portal](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-the-windows-device-portal) on your HoloLens from a web browser.

### Developer mode on a Windows PC

If you're working with a Windows Mixed Reality headset connected to your PC, you must enable **Developer Mode** on the PC.

1.  Go to **Settings**.
2.  Select **Update and Security**.
3.  Select **For developers**.
4.  Enable **Developer Mode**, read the disclaimer for the setting you chose, and then select **Yes** to accept the change.


### Visual Studio dummy project
Create a new empty UWP app in Visual Studio:
   ![](attachments/Pasted%20image%2020220419235618.png)



In VS 2019: Select **Debug; ARM64; Remote Machine;** in the dropdowns

  ![](attachments/Pasted%20image%2020220419235905.png)

Now to connect to the device you need to set the project settings:
![](attachments/Pasted%20image%2020220904174115.png)

![](attachments/Pasted%20image%2020220904174153.png)
In the find window you will need to find your device and select it. It should autodetect on the same network.

### Deploy to Hololens
In **Hololens settings** go to security -> connect for pin
- Press the VS Play button labled "Remote Machine"
- Enter Pin when asked
- UWP should now open after build

After this your PC is connected to the Hololens device portal and you can close Visual Studio and the openend UWP App. 


# Follow the Microsoft tutorial
[Introduction to the Mixed Reality Toolkit--Set Up Your Project and Use Hand Interaction - Learn | Microsoft Docs](https://docs.microsoft.com/en-us/learn/modules/learn-mrtk-tutorials/1-1-introduction?tabs=openxr&ns-enrollment-type=learningpath&ns-enrollment-id=learn.azure.beginner-hololens-2-tutorials)
After completing this tutorial you should have covered all basics of the MRTK installation and usage.

# **<span style="color:red">The rest below is optional and in case the tutorial gets taken down. Everything down this document should already be covered</span>**


# Unity project
![](attachments/Pasted%20image%2020220420000209.png)
Create 3D project with the name you prefer as normal.


Now [download the Mixed Reality Feature Tool from Official Microsoft Download Center](https://www.microsoft.com/en-us/download/details.aspx?id=102778)

In this tool you have to select your created project and select following options:
 ![](attachments/Pasted%20image%2020220420000735.png)
	 - MRTK Foundation
	 - Mixed Reality OpenXR Plugin

Press get features to install them into your project.


# Unity config
In **Project Configuration**:
- "XR Plug-in Management"
- Activate the right-most tab (windows symbol)
- Configure it the following:
 ![](attachments/Pasted%20image%2020220420001205.png)![](attachments/Pasted%20image%2020220420001240.png)


## Build and run settings
To compile and deploy you will need the following settings:
- UWP Platform
- ARM 64 Architecture
- Remote Device via Device Portal
- Device Portal Adress (IP with https)
- Device Portal Username (hololens login)
- Device Portal Password (hololens password) (the password resets after every build and needs to be inputted again)
![](attachments/Pasted%20image%2020220420002022.png)


You must also set login data for the Device Portal. For that see the bottom of Settings -> Developer of the hololens.
And check the https:// link with an IP. There you have to "request Pin" which lets you type a displayed number to set Name and Password of the device Portal. 
These Credentials are those you have to input into the unity window shown above.

