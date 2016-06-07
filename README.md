# Tests-UWP-GC

Output:

```
[38872] GLOBAL:     new LifetimeTester()
[38872] CLASS :     new LifetimeTester()
[38872] LOCAL :     new LifetimeTester()
[38872] REF   :     new LifetimeTester()
[38872] *GC   : after construction
[38872] *GC   :     after start CallMethod(LOCAL )
[38872] LOCAL :     Reference()
[38872] *GC   :         starting NativeCode(LOCAL ) [StillAlive=True]
[29980] LOCAL :     ~LifetimeTester()
[29980] LOCAL :     Dispose(bool)
[38872] *GC   :         NativeCode(LOCAL ) processing [StillAlive=False]
[38872] *GC   :     after end CallMethod(LOCAL )
[38872] *GC   : after localWithNothing
[38872] *GC   :     after start CallMethod(REF   )
[38872] REF   :     Reference()
[38872] *GC   :         starting NativeCode(REF   ) [StillAlive=True]
[38872] *GC   :         NativeCode(REF   ) processing [StillAlive=True]
[38872] *GC   :     after end CallMethod(REF   )
[38872] *GC   : after localWithReference
[29980] REF   :     ~LifetimeTester()
[29980] REF   :     Dispose(bool)
[38872] *GC   : reference: Garbage.LifetimeTester
[38872] *GC   : end
```
