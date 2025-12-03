**其他语言版本: [中文](README.MD), [English](README_EN.MD).**

# 概述

NiuMa 游戏平台是一个全平台全工具链支持，包括 H5、原生 Android 及 IOS、Windows、MAC 等平台，且全平台互通的完整棋牌游戏解决方案，平台支持 Unity3D 和 Cocos Creator(3.8.7)两大主流游戏开发引擎。平台包含游戏服务器(C++)、WEB 服务端（JAVA）、Unity3D 环境客户端、Cocos Creator 环境客户端、后台管理前端五个子项目。NiuMa 是一个免费授权开源项目，任何人都可以自由使用和修改。本项目作为技术学习研究使用，切勿用于非法用途使用，否则由此引发的法律纠纷及连带责任本人概不承担。

项目代码组织入口地址：[Project](https://github.com/niuma-wj)\
掼蛋游戏已更新，欢迎下载客户端测试，\
[下载地址 1](https://github.com/niuma-wj/client/releases/tag/client)\
[下载地址 2](http://106.13.15.226:8896/download)\
[WEB 客户端 PC 版](http://106.13.15.226:8896/niuma/web/pc)\
[WEB 客户端 H5 版](http://106.13.15.226:8896/niuma/web/h5)\
由于 WEB 客户端所在的服务器带宽较低，所以加载相对较慢。

# Unity 版本要求

本项目最低支持的 Unity 版本为 2022.3.12f1 版本，因为依赖的 MessagePack-CSharp 库需要该版本以上才能通过 C#源码生成器以支持 IL2CPP，若不打 IL2CPP(即是用 MONO)则无此要求，不过通常建议打 IL2CPP 包，以便获得更好的性能并同时支持 ARM32 和 ARM64。具体请查看：
[MessagePack-CSharp](https://github.com/MessagePack-CSharp/MessagePack-CSharp#unity-support)

# 热更新方案

本项目使用腾讯开源的 xLua 热更新方案，在此感谢 xLua 开源项目开发者的无私奉献。
xLua 开源项目地址：
[xLua](https://github.com/Tencent/xLua)\
游戏打包之前需要手动生成 xLua 相关的 C#代码，如下图所示：\
![xLua](https://github.com/niuma-wj/data/blob/main/xlua.jpg?raw=true)\

# 游戏打包

打包游戏前需要先把 Assets/NiuMa/Resources 文件夹以及 Assets/NiuMa/Resources.meta 文件临时移出工程，因为 Assets/NiuMa/Resources 文件夹内的所有资源都以 asset bundle 的方式加载，游戏打包完成后再将这两者移回原位置。

# AssetBundle 打包

使用如下图所示菜单打包资源和脚本\
![assetbundle](https://github.com/niuma-wj/data/blob/main/ab.jpg?raw=true)\
AssetBundle 包全部文件如下图所示：\
![assetbundle](https://github.com/niuma-wj/data/blob/main/ab-list.jpg?raw=true)\
其中 MainManifest.xml 文件为客户端主界面依赖的 AssetBundle 包清单，GameManifest.xml 文件为客户端中各游戏依赖的 AssetBundle 包清单，Dependencies.xml 文件为 AssetBundle 包之间的依赖关系，Dependencies.xml 文件在打资源包时自动生成和更新(打 LUA 脚本包不会更新该文件)，其他两个 xml 则需要手动编辑。特别注意，这三个文件都需要保存为 UTF8 编码格式，否则客户端无法正常读取文件内容，所以每次打资源包之后需要手动将 Dependencies.xml 保存为 UTF8 编码格式，例如在 Windows 下可以用记事本打开另存为并选择 UTF8 编码。\
客户端主界面以及各个游戏的依赖 AssetBundle 包可以单独更新，这样使得在更新客户端时可以最小化下载依赖文件，如下图所示：\
![load](https://github.com/niuma-wj/data/blob/main/load.jpg?raw=true)\
![hall](https://github.com/niuma-wj/data/blob/main/hall.jpg?raw=true)

![掼蛋截图 1](https://github.com/niuma-wj/data/blob/main/guandan1.jpg?raw=true)\
![掼蛋截图 2](https://github.com/niuma-wj/data/blob/main/guandan2.jpg?raw=true)

# 启动配置

客户端启动时首先需要加载一个名称为 config.json 的文件，该文件配置了后端 HTTP 接口地址，以及 AssetBundle 加载地址，文件内容如下图所示：\
![config](https://github.com/niuma-wj/data/blob/main/config.jpg?raw=true)\
在 Load.cs 代码中修改 config.json 的下载 url 地址，该 url 是写死在代码中的，当需要访问不同的后端地址时，仅需要重新配置 config.json 并替换该文件，而不需要重新打包客户端，如下图。\
![config1](https://github.com/niuma-wj/data/blob/main/config1.jpg?raw=true)

# Debug 模式

客户端支持调试模式，以便在打包之后(例如安卓和 IOS 端)可以在屏幕上打印 Debug.Log(以及 Lua 的 print 函数)的输出内容，方便在手机上排查错误，开起调试的代码设置如下图：\
![debug](https://github.com/niuma-wj/data/blob/main/debug.jpg?raw=true)\
开启调试模式之后的输出效果：\
![debug](https://github.com/niuma-wj/data/blob/main/debug1.jpg?raw=true)

# 说明

平台当前包含麻将、逮狗腿（斗地主类）、六安比鸡，以及百人牛牛共 4 款棋牌游戏。\
代码仓库中的密钥文件(Assets/Resources/user.keystore)的密码为：123456，该文件在打安卓包时需要用到。

# 联系方式

393817707@qq.com
