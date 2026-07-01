using System;

public interface IButton 
{
    bool IsActive { get; }
    event Action OnStateChanged;
}