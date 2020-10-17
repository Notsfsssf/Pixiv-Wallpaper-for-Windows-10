# Pixiv Wallpaper for Windows 10
[<img src='https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Get_it_from_Microsoft_Badge.svg/320px-Get_it_from_Microsoft_Badge.svg.png' alt='English badge' width=320 height=116/>](https://www.microsoft.com/zh-cn/p/pixiv-wallpaper-for-windows-10/9n71rkg8kcvc?activetab=pivot:overviewtab)

抓取pixiv.net的图片并设置为Windows 10桌面壁纸的UWP APP，需要在Windows 10 1903以上的版本系统中运行。

依赖部分NuGet包，包含简体中文，日文，英文三种语言。

有top50与"猜你喜欢"两种模式，"猜你喜欢"对应网页pixiv.net/discovery 内容，需要登录pixiv账号。

有两种登录模式供选择，一种采用WebView登录，支持外部账号关联登录(如weibo、Twitter账号登录)，该模式下均使用web api进行http通信; 部分Web api使用了[journey-ad](https://github.com/journey-ad)  dalao做的 [pixiv api](https://api.imjad.cn/pixiv.md)  
PixivCS登录模式采用[PixivCS](https://github.com/tobiichiamane/pixivcs/blob/master/PixivAppAPI.cs/ "PixivCS") API，内部大部分使用ios客户端api进行通信。PixivCS模式的代码参考了[pixivUWP](https://github.com/tobiichiamane/pixivfs-uwp/ "pixiv-uwp")项目。十分感谢[鱼](https://github.com/tobiichiamane)在技术上的指导与帮助~  

由于最近pixiv再次更改了API，现在对部分IP来说，web api会存在reCaptcha和hCaptcha两步人机验证……正在考虑是否废除所有的web api方法全部改用pixivcs api并开发其他集合的筛选功能。

## 直连模式
目前Pixiv推荐Ver2模式在中国大陆地区也支持直连pixiv，输入账号密码后可以直接使用。但是图片加载的速度会依个人网络环境而不同，大的图片可能需要有几分钟的加载时间。个人测试时发现联通4G往往会有比较不错的速度。

Top50与Pixiv推荐Ver1模式在中国大陆地区仍需要使用代理服务器并解除UWP应用Loopback限制才能正常使用。

## UWP使用代理
您需要自己准备代理服务器与代理软件，通过代理软件的全局代理模式或者PAC代理模式并 [解除UWP应用Loopback限制](https://sspai.com/post/41137 "UWP loopback")的方式解决UWP应用通过代理连接pixiv.net的问题。

由于pixiv部分url并未被完全封锁，大部分的PAC文件中并没有写入这部分url，可能会出现代理开启的情况下图片加载速度还是很慢的情况。因此在PAC代理模式下，推荐手动将pixiv图床的url “i.pximg.net”与“pixivsketch.net”添加进pac列表中以加速图片下载速度。

## 关于原始项目
初代版本的是由[democyann](https://github.com/democyann)与我共同开发的。(~~实际上当初我只是一个只能写UI和部分简单逻辑还要被琉璃大改逻辑的菜鸡~~)。democyann在2017年之后因为个人原因不再管理此项目，后续开发与维护都由我个人在分支relife完成。在与democyann进行邮件沟通后，她建议我建立自己的代码仓库继续维护此项目。因此今后的Pixiv Wallpaper for Windows 10更新与维护将在此仓库完成，Microsoft Store上面发布的版本也会与此仓库的版本保持一致。

此处为原项目[Pixiv_Wallpaper_for_Win10](https://github.com/democyann/Pixiv_Wallpaper_for_Win10)，已将分支relife与master合并，代码不会再更新。
