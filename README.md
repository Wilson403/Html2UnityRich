# Html4UnityText
支持将一段Html富文本转化为Untiy相关文本显示组件所支持的富文本，目前支持的Unity文本显示组件有UGUI(Text),TextMeshPro。主要思路是使用状态机对整段Html文本进行逐字符分析，并生成语法树，在将语法树按照期望的结果解析为Unity支持的富文本格式



## 目录结构

- Html4UnityText，核心逻辑
- Sample，配套的演示案例
- Plugins，依赖库



## 关键代码说明



