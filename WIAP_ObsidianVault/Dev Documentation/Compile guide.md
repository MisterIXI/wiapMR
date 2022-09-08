# Hololens
The Hololens build is an UWP app compiled on ARM and can't be executed nor compiled on the local machine. This is why we do something called "remote compile":
When the device portal is setup correctly according to the middle steps described and linked in [IDE Setup](IDE%20Setup.md) you can compile on your hololens. For that it needs to be turned on, and in the same network as you.
![](attachments/Pasted%20image%2020220908223646.png)
The platform version can be different, but you need to copy the rest of the settings.
You should always **Build and Run** to correctly push the finished build into the app folder of the hololens. It might not be there afterwards otherwise.
Once built, it automatically starts on the hololens and the app can be found in "all apps". 

Note: it overrides the last build pushed to it as long as the names stays the same.

# Windows VR build
The windows build is straight forward since its a "normal" build with unity default settings:
![](attachments/Pasted%20image%2020220908223829.png)

Here you can "Build" or "Build and Run" as you prefer. It outputs the actual build with the excutable in it to your selected folder.
Remember to put the Games folder from assets into the unity data folder. (`[build]\MR_Unity_Data\_Games\`) Otherwise the import does not work.