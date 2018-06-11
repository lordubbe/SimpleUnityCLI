using System;

public abstract class SyntaxToken
{
    public virtual bool Match(SyntaxToken nextToken)
    {
        if (ValidNextTypes != null)
        {
            Type nextTokenType = nextToken.GetType();
            for (int i = 0; i < ValidNextTypes.Length; i++)
            {
                if (nextTokenType == ValidNextTypes[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected abstract Type[] ValidNextTypes { get; }

    public static SyntaxToken GetTokenByString(string tokenName)
    {
        return null;
    }
}

public class CommandStartToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(KeywordToken), typeof(ObjectReferenceToken), typeof(PropertyToken), typeof(MethodToken) };
        }
    }
}

public class KeywordToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(PropertyToken) };
        }
    }
}

public class ObjectReferenceToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(KeywordToken), typeof(MethodToken) };
        }
    }
}

public class PropertyToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(ParameterToken) };
        }
    }
}

public class FieldToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(ParameterToken) };
        }
    }
}

public class MethodToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(ParameterToken) };
        }
    }
}

public class ParameterToken : SyntaxToken
{
    protected override Type[] ValidNextTypes
    {
        get
        {
            return new Type[] { typeof(ParameterToken) };
        }
    }
}

public class SetKeyword : KeywordToken { }
