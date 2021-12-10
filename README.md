# 111級畢業專題 - MR室內導覽
### 混合實境與室內定位之應用
<br/>
指導教授：林士勛 博士<br/>
參與成員：徐浩倫 沈奎宏 姜天皓 趙志維 洪世穎
<hr/>

此專案為 Unity 專案 <br/>
Unity Version: <b>2020.3.2f1</b>

### 學生撰寫的程式碼：
[CoordinateTransform.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/CoordinateTransform.cs)

[OutputJson.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/OutputJson.cs)

[Pathfinding.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/Pathfinding.cs)

[Nav.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/Nav.cs)

<hr/>

## 版本紀錄：
### v0.6
新增智慧助手

### v0.5
修正導航功能在使用AR模式後，於VR模式無法使用的問題

### v0.4
新增VR導覽模式
<br/>修正導航路徑未更新的問題

### v0.3
新增[NodeInfo.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/main/Assets/NodeInfo.cs), [OutputJson.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/OutputJson.cs)
<br/>完成自動 JSON 輸出節點資訊功能

### v0.2
新增[Nav.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/Nav.cs), [Pathfinding.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/Pathfinding.cs)
<br/>初步完成導航功能

### v0.1
新增 [CoordinateTransform.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/CoordinateTransform.cs), 路徑：[V-AR/Assets/CoordinateTransform.cs](https://github.com/golddragon11/MR-Indoor-Touring/blob/master/Assets/CoordinateTransform.cs)
<br/>使用方法請見註解
<br/>此 script 已加在 GameObject IndoorAtlasSession 之下