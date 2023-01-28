## Html2UnityRich
支持将一段Html富文本转化为Untiy相关文本显示组件所支持的富文本，目前支持的Unity文本显示组件有UGUI(Text),TextMeshPro。主要思路是使用状态机对整段Html文本进行逐字符分析，并生成语法树，在将语法树按照期望的结果解析为Unity支持的富文本格式



## 目录结构

- Html4UnityText，核心逻辑
- Sample，配套的演示案例
- Plugins，依赖库



## 部分代码说明

- HtmlMachine.cs， 解析Html文本用的状态机，拆分出标签以及纯文本
- TagPropMachine.cs， 解析Html标签用的状态机，获取具体参数
- HtmlNode.cs，构成语法树的节点基础对象
- Html2UnityRichMgr.cs,  接口入口文件
- Start.cs，演示代码文件



## 如何运行演示案例

打开SampleScene.scene场景文件，点击运行



## 如何使用代码

start.cs包含了具体2种Text组件（UGUI和TextMeshPro）的使用逻辑

```c#
//textMeshPro演示
string content = "<h1>这是一段Html文本</h1>"
textMeshPro.text = Html2UnityRichMgr.CreateHtmlRootNode (content).ToPropNode ().ToUnityRichNode ().ToTextProRichText ();
```

UGUI(Text)由于不带对齐的富文本的标签，所以实现多了一些步骤，具体实现见Start.cs

## 适用的场景
我自己的需求是用于游戏的公告，游戏公告一般是运营人员编写发出的，他们更多熟悉Html，而不了解Unity的富文本，所以写了这个库来进行转换

## 后续更新计划
- 支持图文混排
- <s>支持Html超链接标签（已实现）</s>

## 演示效果
![image](https://user-images.githubusercontent.com/38308449/206893926-f05ac168-0b64-4d25-bc3e-c2e944f91464.png)

## 推荐项目
  - [JEngine](https://github.com/JasonXuDeveloper/JEngine) - The solution that allows unity games update in runtime. 使Unity开发的游戏支持热更新的解决方案。
  - [BDFramework](https://github.com/yimengfan/BDFramework.Core) - Simple and powerful Unity3d game workflow! 简单、高效、高度工业化的商业级unity3d 工作流。
