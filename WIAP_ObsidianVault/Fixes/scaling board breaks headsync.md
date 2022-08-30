different scaling of gameboard results in broken headsync positions. It is possible to fix properly, but it complicates things a lot more. This has been decided to be out of scope, but to fix you would need to calculate the difference in movement according to difference of boardhelper scale vs board scale.

The applied workaround was disabling two handed scaling in MRTK:
```csharp
var om = boardObj.AddComponent<ObjectManipulator>();

om.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
```