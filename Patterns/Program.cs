using Patterns.Creation;
using Patterns.Solid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    

    class Program
    {
        static void Main(string[] args)
        {
            // SOLID

            var S = new SingleResponsibility();
            //S.Test();

            var O = new OpenClosed();
            //O.Test();

            var L = new Liskov();
            L.Test();

            var I = new InterfaceSegregation();
            I.Test();

            var D = new DependencyInversion();
            D.Test();

            // CREATION
            // Builder
            var builder = new Builder();
            builder.Test();

            var builderFluent = new BuilderFluentWithGenerics();
            builderFluent.Test();

            var builderFacade = new BuilderFaceted();
            builderFacade.Test();

            var builderExercise = new BuilderExercise();
            builderExercise.Test();

            // Factory
            var factoryPoint = new FactoryPointWrong();
            factoryPoint.Test();

            var factoryMethod = new FactoryMethod();
            factoryMethod.Test();

            var factoryClass = new Creation.FactoryClass.FactoryClass();
            factoryClass.Test();

            var factoryInner = new Creation.FactoryInner.FactoryInner();
            factoryInner.Test();

            var factoryAbstract01 = new Creation.FactoryAbstract01.FactoryAbstract01();
            //factoryAbstract01.Test();

            var factoryAbstract02 = new Creation.FactoryAbstract02.FactoryAbstract02();
            //factoryAbstract02.Test();

            var factoryExercise = new Creation.FactoryExercise.FactoryExercise();
            factoryExercise.Test();

            // Prototype
            Patterns.Creation.Prototype.P01.Prototype01.Test();
            Patterns.Creation.Prototype.P02.Prototype02.Test();
            Patterns.Creation.Prototype.P03.Prototype03.Test();
            Patterns.Creation.Prototype.Exercise.PrototypeExercise.Test();

            // Singleton
            Patterns.Creation.Singleton.S01.Singleton.Test();
            Patterns.Creation.Singleton.S02.Monostate.Test();

            // STRUCTURE
            // Adapter
            Patterns.Structure.Adapter.A01.AdapterNoCaching.Test();
            Patterns.Structure.Adapter.A02.AdapterWithCaching.Test();

            // Bridge
            Patterns.Structure.Bridge.Bridge.Test();

            // Composite
            Patterns.Structure.Composite.CompositeShapes.Test();
            Patterns.Structure.Composite.Exercise.Exercise.Test();

            // Decorator
            Patterns.Structure.Decorator.CustomStringBuilder.Test();
            Patterns.Structure.Decorator.DecoratorAdapter.Test();
            Patterns.Structure.Decorator.DecoratorMultipleInheritance.Test();
            Patterns.Structure.Decorator.DynamicDecorator.Test();
            Patterns.Structure.Decorator.Static.StaticDecorator.Test();

            // Flyweight
            Patterns.Structure.Flyweight.Flyweight01.Test();
            Patterns.Structure.Flyweight.Flyweight02.Test();
            Patterns.Structure.Flyweight.Exercise.Test();

            // Proxy
            Patterns.Structure.Proxy.ProtectionProxy.Test();
            Patterns.Structure.Proxy.PropertyProxy.Test();
            Patterns.Structure.Proxy.DynamicProxy.Test();

            // Chain of responsibility
            Patterns.Structure.ChainOfResponsibility.C01.Chain01.Test();
            Patterns.Structure.ChainOfResponsibility.C02.Chain02.Test();

            // Command
            Patterns.Structure.Command.Command01.Test();

            // Interpreter
            Patterns.Structure.Interpreter.Interpreter01.Test();
            Patterns.Structure.Interpreter.Exercise.Exercise.Test();

            // Iterator
            Patterns.Structure.Iterator.Iterator01.Test();
            Patterns.Structure.Iterator.Exercise.Exercise.Test();

            // Mediator
            Patterns.Structure.Mediator.Chat.Test();
            Patterns.Structure.Mediator.EventBrokerExample.Test();
            Patterns.Structure.Mediator.Example.Test();

            // Memento
            Patterns.Structure.Memento.Memento01.Test();
            Patterns.Structure.Memento.M02.Memento02.Test();

            // Null Object
            Patterns.Structure.NullObject.Null01.Test();

            // Observer
            Patterns.Structure.Observer.Ob01_ViaEvent.Test();
            Patterns.Structure.Observer.Ob02_WeakEvent.Test();
            Patterns.Structure.Observer.Ob3_Observable.Test();
            Patterns.Structure.Observer.Exercise.Test();

            // State
            //Patterns.Structure.StateMachine.HandMade.Test();
            //Patterns.Structure.StateMachine.Switch.SwitchBased.Test();

            // Strategy
            Patterns.Structure.Strategy.Dynamic.Test();
            Patterns.Structure.Strategy.Static.Static.Test();

            // Template Method
            Patterns.Structure.TemplateMethod.Method.Test();

            // Observer
            Patterns.Structure.Visitor.V01_Instusive.Test();
            Patterns.Structure.Visitor.V02_ReflectionBased.Test();
            Patterns.Structure.Visitor.V03_ClassicVisitor.Test();
            Patterns.Structure.Visitor.V04_DynamicVisitorDLR.Test();
            Patterns.Structure.Visitor.V05_AcyclicVisitor.Test();
            Patterns.Structure.Visitor.Exercise.Test();

            // Bonus
            Patterns.Bonus.ContinuationPassingStyle.Test();

            System.Console.ReadLine();
        }
    }
}
