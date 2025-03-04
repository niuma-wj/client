# Unity版本要求
本项目最低支持的Unity版本为2022.3.12f1版本，因为依赖的MessagePack-CSharp库需要该版本以上才能通过C#源码生成器以支持IL2CPP，若不打IL2CPP(即是用MONO)则无此要求，不过通常建议打IL2CPP包，以便获得更好的性能并同时支持ARM32和ARM64。具体请查看：
[MessagePack-CSharp](https://github.com/MessagePack-CSharp/MessagePack-CSharp#unity-support)

# 热更新方案
本项目使用腾讯开源的xLua热更新方案，在此感谢xLua开源项目开发者的无私奉献。
xLua开源项目地址：
[xLua](https://github.com/Tencent/xLua)

# AssetBundle打包
使用如下图所示菜单打包资源和脚本\
![assetbundle](https://gitee.com/friedrich-hegel/data/raw/master/ab.png)\
AssetBundle包全部文件如下图所示：\
![assetbundle](https://gitee.com/friedrich-hegel/data/raw/master/ab-list.png)\
其中MainManifest.xml文件为客户端主界面依赖的AssetBundle包清单，GameManifest.xml文件为客户端中各游戏依赖的AssetBundle包清单，Dependencies.xml文件为AssetBundle包之间的依赖关系，Dependencies.xml文件在打资源包时自动生成和更新(打LUA脚本包不会更新该文件)，其他两个xml则需要手动编辑。特别注意，这三个文件都需要保存为UTF8编码格式，否则客户端无法正常读取文件内容，所以每次打资源包之后需要手动将Dependencies.xml保存为UTF8编码格式，例如在Windows下可以用记事本打开另存为并选择UTF8编码。\
客户端主界面以及各个游戏的依赖AssetBundle包可以单独更新，这样使得在更新客户端时可以最小化下载依赖文件，如下图所示：\
![load](https://gitee.com/friedrich-hegel/data/raw/master/load.png)\
![hall](https://gitee.com/friedrich-hegel/data/raw/master/hall.png)

# 启动配置
客户端启动时首先需要加载一个名称为config.json的文件，该文件配置了后端HTTP接口地址，以及AssetBundle加载地址，文件内容如下图所示：\
![config](https://gitee.com/friedrich-hegel/data/raw/master/config.png)\
在Load.cs代码中修改config.json的下载url地址，该url是写死在代码中的，当需要访问不同的后端地址时，仅需要重新配置config.json并替换该文件，而不需要重新打包客户端，如下图。\
![config1](https://gitee.com/friedrich-hegel/data/raw/master/config1.png)

# Debug模式
客户端支持调试模式，以便在打包之后(例如安卓和IOS端)可以在屏幕上打印Debug.Log(以及Lua的print函数)的输出内容，方便在手机上排查错误，开起调试的代码设置如下图：\
![debug](https://gitee.com/friedrich-hegel/data/raw/master/debug.png)\
开启调试模式之后的输出效果：\
![debug](https://gitee.com/friedrich-hegel/data/raw/master/debug.png)

# 测试客户端
[下载地址](http://106.13.15.226:8896/download)

# 说明
代码仓库中的密钥文件(Assets/Resources/user.keystore)的密码为：123456，该文件在打安卓包时需要用到

# 联系方式
393817707@qq.com