   MEF加载器
  通过MEF获取插件  
  
  ----------------------------
  
  1.定义多个接口，为各类插件使用。
  2.插件类型：  
    IMainView为主插件  
	IView一般子插件  
	IAgrs插件，共有启动插件，一开始就启动  
	IPlugin插件，一般业务插件   
  3.界面UI插件开发方式  
    （1）插件dll中实现接口，通过该接口提供自己插件的窗体   
	（2）每个插件按照配置名称获取  
	（3）每个插件有一个显示名称，作为界面显示名称，当前例子用的同一个 
  4.样例  
     <p align='center'>
    <img src='1.jpg' title='images' style='max-width:600px'></img>
    </p>
	 <p align='center'>
    <img src='2.jpg' title='images' style='max-width:600px'></img>
    </p>
  5.逻辑过程  
   <p align='center'>
    <img src='3.jpg' title='images' style='max-width:600px'></img>
    </p>
     
	  
  
	
