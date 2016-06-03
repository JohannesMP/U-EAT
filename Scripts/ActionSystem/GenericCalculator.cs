/****************************************************************************/
/*!
\file   GenericCalculator.cs
\author Joshua Biggs
\par    Email: Joshua.B@Digipen.edu
\par    Developed: Summer 2016
\brief

This file contains the static GenericCalculator class. When the user accesses it,
this class finds the basic arithmatic operator functions with given declarations.
If these operator functions do not exist, the user say to use their own functions
instead. 

© 2016 Joshua Biggs CC Attribution
*/
/****************************************************************************/
using System;
using System.Linq.Expressions;

public static class GenericCalculator<T1, T2, TResult>
{
    static public Func<T1, T2, TResult> AddFunc = null;
    static public Func<T1, T2, TResult> SubtractFunc = null;
    static public Func<T1, T2, TResult> MultiplyFunc = null;
    static public Func<T1, T2, TResult> DivideFunc = null;

    static GenericCalculator()
    {
        var leftType = typeof(T1);
        var rightType = typeof(T2);
        ParameterExpression leftVal = Expression.Parameter(leftType, "leftVal");
        ParameterExpression rightVal = Expression.Parameter(rightType, "rightVal");
        
        try
        {
            BinaryExpression addExp = Expression.Add(leftVal, rightVal);
            AddFunc = Expression.Lambda<Func<T1, T2, TResult>>(addExp, leftVal, rightVal).Compile();
        }
        catch (InvalidOperationException){ }
        catch (ArgumentException) { }
        try
        {
            BinaryExpression subExp = Expression.Subtract(leftVal, rightVal);
            SubtractFunc = Expression.Lambda<Func<T1, T2, TResult>>(subExp, leftVal, rightVal).Compile();
        }
        catch (InvalidOperationException) { }
        catch (ArgumentException) { }
        try
        {
            BinaryExpression multExp = Expression.Multiply(leftVal, rightVal);
            MultiplyFunc = Expression.Lambda<Func<T1, T2, TResult>>(multExp, leftVal, rightVal).Compile();

        }
        catch (InvalidOperationException) { }
        catch (ArgumentException) { }
        try
        {
            BinaryExpression divExp = Expression.Divide(leftVal, rightVal);
            DivideFunc = Expression.Lambda<Func<T1, T2, TResult>>(divExp, leftVal, rightVal).Compile();
        }
        catch (InvalidOperationException) { }
        catch (ArgumentException) { }
    }

    static public TResult Add(T1 lhs, T2 rhs)
    {
        if(AddFunc != null)
        {
            return AddFunc(lhs, rhs);
        }
        throw new Exception("There is no known operation: " + 
                             typeof(T1).Name + " + " + typeof(T2).Name + " = " + typeof(TResult).Name + 
                             "\nPerhaps you need to set one?");
    }

    static public TResult Subtract(T1 lhs, T2 rhs)
    {
        if (SubtractFunc != null)
        {
            return SubtractFunc(lhs, rhs);
        }
        throw new Exception("There is no known operation: " +
                             typeof(T1).Name + " - " + typeof(T2).Name + " = " + typeof(TResult).Name +
                             "\nPerhaps you need to set one?");
    }

    static public TResult Multiply(T1 lhs, T2 rhs)
    {
        if (MultiplyFunc != null)
        {
            return MultiplyFunc(lhs, rhs);
        }
        throw new Exception("There is no known operation: " +
                             typeof(T1).Name + " * " + typeof(T2).Name + " = " + typeof(TResult).Name +
                             "\nPerhaps you need to set one?");
    }

    static public TResult Divide(T1 lhs, T2 rhs)
    {
        if (DivideFunc != null)
        {
            return MultiplyFunc(lhs, rhs);
        }
        throw new Exception("There is no known operation: " +
                             typeof(T1).Name + " / " + typeof(T2).Name + " = " + typeof(TResult).Name +
                             "\nPerhaps you need to set one?");
    }
}