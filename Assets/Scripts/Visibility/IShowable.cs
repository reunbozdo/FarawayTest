using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShowable
{
    public void Show(bool immediately, Action callback);
    public void Hide(bool immediately, Action callback);
}
