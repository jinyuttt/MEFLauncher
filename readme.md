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
     (1)  
	 ![image](https://github.com/jinyuttt/MEFLauncher/blob/master/screenshots/1.png)  
	 (2)  
	  ![image](https://github.com/jinyuttt/MEFLauncher/blob/master/screenshots/2.png)  
  5.逻辑过程  
      ![image](https://github.com/jinyuttt/MEFLauncher/blob/master/screenshots/3.png)  
	  
  
	
