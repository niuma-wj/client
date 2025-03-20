**Read this in other languages: [English](README_EN.MD), [中文](README.MD).**

# Unity version requirement
Due to the using of MessagePack-CSharp library which is a message serialization solution, the minimum supported Unity version is 2022.3.12f1, as it is necessary to support IL2CPP via C# Source Generator. There's no such requirement if you don't build the client of IL2CPP version (that is building Mono version), But the IL2CPP version is recommended, for it can support both ARM32 and ARM64, and will have a better performance. Here is the Github address of the MessagePack-CSharp library.\
[MessagePack-CSharp](https://github.com/MessagePack-CSharp/MessagePack-CSharp#unity-support)

# Dynamic update solution
We use the open source project xLua which is developed by Tencent as the dynamic update solution for this client, thank you to the developers of the xLua open source project for their dedication. Here is the Github address of the xLua project.\
[xLua](https://github.com/Tencent/xLua)

# AssetBundle package
Here is the command menu for packaging game resources and Lua scripts.\
![assetbundle](https://gitee.com/friedrich-hegel/data/raw/master/ab.png)\
All the asset-bundle files are shown in the following picture.\
![assetbundle](https://gitee.com/friedrich-hegel/data/raw/master/ab-list.png)\
The MainManifest.xml file is the manifest of the dependent asset-bundle files of this client's login and main interface, all the dependent asset-bundle files of each game in this client are listed in the GameManifest.xml file, the dependencies between all asset-bundle files are listed in the Dependencies.xml file. When packaging game resources the Dependencies.xml file will be generated and updated automatically, the Dependencies.xml file will not change when packaging Lua scripts, the other two xml files need to be edited manually. In particular, these three xml files need to be saved as UTF8 encoding format, otherwise the client cannot read the file content normally, so after the Dependencies.xml file is modified by the packaging program, you need to save it as UTF8 encoding format manually, for example, under Windows, you can use Notepad to open it then save as and select UTF8 encoding.\
The asset-bundle files listed in MainManifest.xml and Dependencies.xml can be downloaded independently, this minimizes the download of dependent files when updating the client, as shown in the following pictures.\
![load](https://gitee.com/friedrich-hegel/data/raw/master/load.png)\
![hall](https://gitee.com/friedrich-hegel/data/raw/master/hall.png)

# Startup Configuration
When the client startup, it will load a configuration file named "config.json", this file configures the host of the web server and the host of the asset-bundle file server, the content in this configuration file is shown in the following picture.\
![config](https://gitee.com/friedrich-hegel/data/raw/master/config.png)\
The download url for config.json is specified in the source code file Load.cs, which is hard coded in the source file, when the back-end servers are migrated, we can just update the config.json file, and we don't need to repack and republish the client. \
![config1](https://gitee.com/friedrich-hegel/data/raw/master/config1.png)

# Debug Mode
The client supports debug mode, so that after packaging (such as Android and IOS) you can print the output of the Unity function "Debug.log" (or Lua's "print" function) on the screen, which is convenient for debuging errors on the phone. The debug-mode setting code are as follows.\
![debug](https://gitee.com/friedrich-hegel/data/raw/master/debug.png)\
The output effect after debug-mode is enabled.\
![debug](https://gitee.com/friedrich-hegel/data/raw/master/debug1.png)

# Testing client
A testing client is available on the Internet, every one is welcomed to download and test it.\
[client](http://106.13.15.226:8896/download)

# Note
The password of the key store file (Assets/Resources/user.keystore) in this project is "123456", the key store file is needed when building android client.

# Contact
393817707@qq.com