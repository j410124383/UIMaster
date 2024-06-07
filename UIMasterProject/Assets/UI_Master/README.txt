2024.06.07
欢迎使用UImaster，这个组件可以帮助游戏制作者专注于游戏玩法本身。
将UI相关的工作交由我们来先完成。
制作者只需要替换对应的素材即可。

目前已接入功能：
图鉴系统
设置系统
金手指系统
手柄控制系统

官方网站：
可查询更多信息
https://uimaster.vercel.app/


## 操作准备

### 文本相关预设

1. 导入TMP Essentials：Edit > Project Setting > Text Mesh Pro > Import TMP Essentials。
   - 需要先安装TMP的预设。

2. 富文本设置：
   - 在Project Setting > TextMesh Pro > Settings中，将Default Sprite Asset替换为`SpriteAsset_Default`。
   - 将Default Style Sheet替换为`StyleSheet_Default`。
   - 将Default Font Asset替换为`NotoSansSC-Black SDF`文件。当然，后续可以根据项目需求替换为其他字体。

### 本地化相关预设

1. 在Package Manager中安装Localization本地化插件。
2. 在Project Setting > Localization中，将默认的本地化文件替换上去。

### 输入系统相关预设

1. 在Project Setting > Api Compatibility Level中将替换为.NET Framework。
2. 将Active Input Handling替换为Input System Package。
   - 我们使用新的控制系统来支持项目中的UI设置。
   - 在Package Manager中安装Input System。

### 渲染管线相关预设

1. 在Package Manager中安装Universal RP（通用渲染管线）。
2. 在Project Setting > URP Global Settings中，将已有的Rendering Asset资产拖拽进去。如果有问题，可以在该页面进行修复。
   - 这套方案会与URP绑定，所以需要适配。如果不使用URP，需要在SettingManager中将Value命名空间相关的代码注释或删除。

### 建议操作

1. 在Project Setting中，将Color Space改为Linear。这可能对游戏的美术效果有影响，为了确保最佳品质。

