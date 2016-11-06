/****************************************************************************/
/*!
\file   Property.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

The Property class contains the getter and setter for a variable that exists somewhere.
Properties are used to change the target value passed into the action system without requiring the user to set anything.
The extension methods can be used to get the getter, setter, or Property for a variable.
The variables that can be used to make a property with MUST be public and non-readonly.

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using System;
using System.Linq.Expressions;
using System.Reflection;

public class Property<T>
{
    private Func<T> Getter;
    private Action<T> Setter;

    public Property(Func<T> getter, Action<T> setter)
    {
        Getter = getter;
        Setter = setter;
    }

    public T Get()
    {
        return Getter();
    }

    public void Set(T value)
    {
        Setter(value);
    }
}

public static class PropertyExtensions
{
    public static Action<T> GetPropertySetter<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }
        return GetPropertySetter<TObject, T>(instance, accessedMember);
    }

    public static Action<T> GetPropertySetter<TObject, T>(this TObject instance, PropertyInfo info)
    {
        var setter = info.GetSetMethod();
        return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, setter);
    }

    public static Func<T> GetPropertyGetter<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }

        return GetPropertyGetter<TObject, T>(instance, accessedMember);
    }

    public static Func<T> GetPropertyGetter<TObject, T>(this TObject instance, PropertyInfo info)
    {
        var getter = info.GetGetMethod();
        return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), instance, getter);
    }

    public static Property<T> GetProperty<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null)
        {
            throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");
        }

        var getter = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), instance, accessedMember.GetGetMethod());
        var setter = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, accessedMember.GetSetMethod());

        return new Property<T>(getter, setter);
    }
}
