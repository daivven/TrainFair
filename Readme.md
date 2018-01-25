# 设计模式之策略模式在地铁票价系统中的应用 #
##  引言 ##
设计模式是面向对象编程的一个非常精彩的部分。使用设计模式是为了可重用代码、让代码更容易被他人理解、保证代码可靠性，它能帮助我们将应用组织成容易了解，容易维护，具有弹性的架构。本文通过一个简单的案例来讲述策略模式在地铁票价系统中的应用。

## 案例描述  ##

乘客从一个车站乘坐地铁到另一个车站，他/她需要购买一张票。铁路部门对于票价有一些特别的票价规定： 

> 按照市物价主管部门批复的轨道交通网络票价体系，即：轨道交通实行按里程计价的多级票价，0~6公里3元，6公里之后每10公里增加1元;票价计算采用最短路径法，即：当两个站点之间有超过1条换乘路径时，选取里程最短的一条路径作为两站间票价计算依据。 

## 案例分析 ##

让我们考虑有20个站点：1s，2s，3s......20s，并且乘客在不同的场景下乘坐地铁。为了更清晰的讲述问题，我们在原有定价标准上虚拟了一些应用场景。


- 如果乘客A乘坐的里程小于6公里，那么他将需要支付3元车票费用。


- 如果乘客B乘坐的里程大于6公里，他将需要额外支付超出部分的车票费用，计费标准为6公里之后每10公里增加1元。


- 如果乘客C是VIP客户，那么他将在原计费标准上享受9折优惠。

- 如果后续有一些额外收费或额外优惠，在以上计费基础上再进行调整。

## 解决方案 ##

这个问题可以通过使用“策略设计模式”来解决。因为不同类型的票价策略可以基于不同的规则来应用。 以下是票价策略的不同类型：
- 基本票价规则战略
- VIP票价规则策略
- 额外的票价规则策略

每张票价规则策略将分别写入票价计算算法，这些算法不会相互干扰。 新的票价规则可以添加和写入新的票价规则策略。这种模式也将遵循“对扩展开放、对修改关闭”的理念。

**依赖关系图**
![](https://i.imgur.com/4yrVtyG.png)
**类图**
![](https://i.imgur.com/iKagQ4Z.png)

**代码说明**

**IFareStrategy接口**

这个接口定义了票价计算的常用策略，实现一个类可以实现基于上下文的票价算法。
```
using TrainFair.Models;    
namespace TrainFair.FareCalculator  
{  
    public interface IFareStrategy {  
        float GetFare(IFareRule ruleValues, float basicFare);  
    }  
}  
```
**FareConstants类**
FareConstants定义了计费的规则，包括起步价，超出里程递增价及VIP折扣价。
```
namespace TrainFair.Constants
{
    public class FareConstants {
        public const float BasicFare = 3.0F;
        public const float OnStationFare = 1.0F;
        public const float VIPDiscount = 0.1F;
    }
}
```
**StationRuleFareCalculator类**

StationRuleFareCalculator类根据行驶的车站里程和问题陈述部分定义的一些规则集来计算车费。
```
using System;  
using TrainFair.Models;  
  
namespace TrainFair.FareCalculator  
{  
    public class StationRuleFareCalculator : IFareStrategy  
    {  
        public float GetFare(IFareRule ruleValues, float basicFare) {  

            var stationFareRuleModel = ruleValues as StationFareRuleModel;  
             if (stationFareRuleModel == null || stationFareRuleModel.StationDistance <= 0.0f)
                return 0;
            
            if (stationFareRuleModel.StationDistance < 6)
                return basicFare;

            int restChargingStations = (int)Math.Ceiling((stationFareRuleModel.StationDistance - 6.0f)/10.0f);
            var totalFare = basicFare + restChargingStations * stationFareRuleModel.IncrementalPrice;           

            return totalFare;
        }  
    }  
}  
```
**VIPRuleFareCalculator类**
这个类实现的是VIP的票价算法。如果乘客是VIP身份，那么他/她将得到享受特殊的优惠。这个类实现了这个算法。
```
using TrainFair.Models;  
  
namespace TrainFair.FareCalculator  
{  
     public class VIPRuleFareCalculator : IFareStrategy
    {
        public float GetFare(IFareRule ruleValues, float basicFare) {
            var vipFareRuleModel = ruleValues as VIPFareRuleModel;
            if (vipFareRuleModel == null)
                return 0;
           
            var totalFare = basicFare - (basicFare * vipFareRuleModel.Discount);
            return totalFare;
        }
    }
}   
```

**OtherRuleFareCalculator类**
这个类实现的是其他额外的费用或优惠票价的算法。一些额外的价格将被添加到总费用中。额外的价格可以是附加收费（正值），也可以是额外折扣（负值）。
```
using TrainFair.Models;  
  
namespace TrainFair.FareCalculator  
{  
    public class OtherRuleFareCalculator : IFareStrategy
    {
        public float GetFare(IFareRule ruleValues, float basicFare) {
            var otherFareRuleModel = ruleValues as OtherFareRuleModel;
            if (otherFareRuleModel == null)
                return basicFare;

            float totalFare = basicFare + otherFareRuleModel.AdditionalFare;
            return totalFare;
        }
    }
}  
```
**FareRuleCalculatorContext类**
```
using TrainFair.Models;  
  
namespace TrainFair.FareCalculator  
{  
    public class FareCalculatorContext {  
  
        private readonly IFareStrategy _fareStrategy;  
        public FareCalculatorContext(IFareStrategy fareStrategy) {  
            this._fareStrategy = fareStrategy;  
        }  
  
        public float GetFareDetails(IFareRule fareRules, float basicFare)  
        {  
            return _fareStrategy.GetFare(fareRules, basicFare);  
        }  
    }  
}  
```

代码结构里有一些基于车站票价，VIP票价，额外票价等情况的model类。

**IFareRule接口**

这是基本票价规则模型接口，每个模型类都实现它。
```
namespace TrainFair.Models  
{  
    public interface IFareRule  
    {  
        int FareRuleId { get; set; }  
    }  
}  
```
**StationFareRuleModel类**
这个类定义的是车站票价规则的基本属性。
```
namespace TrainFair.Models  
{  
    public class StationFareRuleModel : IFareRule  
    {  
        public int FareRuleId { get; set; }

        public int  StationsCounts { get; set; }

        public float IncrementalPrice { get; set; }

        public float StationDistance { get; set; }  
    }  
} 
``` 
**VIPFareRuleModel类**

这个类定义了VIP折扣的属性。
```
namespace TrainFair.Models  
{  
    public class VIPFareRuleModel : IFareRule  
    {  
        public int FareRuleId { get; set; }       

        public float Discount { get; set; }
    }  
}  
```
**OtherFareRuleModel类**

这个类定义其他额外收费的属性。
```
namespace TrainFair.Models  
{  
    public class OtherFareRuleModel : IFareRule  
    {  
        public int FareRuleId { get; set; }  
  
        public string OtherFareName { get; set; }  
  
        public float AdditionalFare { get; set; }  
    }  
}  
```
模型的属性可以根据未来的需求进行增强和调整，并可以灵活应用在算法类中。

**执行结果**

以下是控制台输出：
![](https://i.imgur.com/lIXYFnR.jpg)
 
本文结尾附上了程序代码。 


## 结语 ##

车站基础票价、VIP票价、额外票价等不同类型的票价计算规则是不同的，所有的算法都被分解到不同的类中，以便能够在运行时选择不同的算法。策略模式的用意是针对一组算法或逻辑，将每一个算法或逻辑封装到具有共同接口的独立的类中，从而使得它们之间可以相互替换。策略模式使得算法或逻辑可以在不影响到客户端的情况下发生变化。说到策略模式就不得不提及OCP(Open Closed Principle) 开闭原则，即对扩展开放，对修改关闭。策略模式的出现很好地诠释了开闭原则，有效地减少了分支语句。

**程序代码**：<a href="https://github.com/daivven/TrainFair">https://github.com/daivven/TrainFair</a>



<div style="background: #f0f8ff; padding: 10px; border: 2px dashed #990d0d; font-family: 微软雅黑;">
&nbsp;作者：<a href="http://www.cnblogs.com/yayazi/">阿子</a>
<br>
&nbsp;博客地址：<a href="http://www.cnblogs.com/yayazi/">http://www.cnblogs.com/yayazi/</a>
<br>
&nbsp;本文地址：<a href="http://www.cnblogs.com/yayazi/p/8350679.html">http://www.cnblogs.com/yayazi/p/8350679.html</a>
<br>
&nbsp;声明：本博客原创文字允许转载，转载时必须保留此段声明，且在文章页面明显位置给出原文连接。

</div>