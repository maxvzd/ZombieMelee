using UnityEngine;

public static class RayCastDebug
{
    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color, float duration)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color, duration);
    }
    
    private static void DrawBox(Box box, Color color, float duration)
    {
        Debug.DrawLine(box.FrontTopLeft,	 box.FrontTopRight,	color, duration);
        Debug.DrawLine(box.FrontTopRight,	 box.FrontBottomRight, color, duration);
        Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color, duration);
        Debug.DrawLine(box.FrontBottomLeft,	 box.FrontTopLeft, color, duration);
												 
        Debug.DrawLine(box.BackTopLeft,		 box.BackTopRight, color, duration);
        Debug.DrawLine(box.BackTopRight,	 box.BackBottomRight, color, duration);
        Debug.DrawLine(box.BackBottomRight,	 box.BackBottomLeft, color, duration);
        Debug.DrawLine(box.BackBottomLeft,	 box.BackTopLeft, color, duration);
												 
        Debug.DrawLine(box.FrontTopLeft,	 box.BackTopLeft, color, duration);
        Debug.DrawLine(box.FrontTopRight,	 box.BackTopRight, color, duration);
        Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color, duration);
        Debug.DrawLine(box.FrontBottomLeft,	 box.BackBottomLeft, color, duration);
    }

    public struct Box
	{
		public Vector3 LocalFrontTopLeft     {get; private set;}
		public Vector3 LocalFrontTopRight    {get; private set;}
		public Vector3 LocalFrontBottomLeft  {get; private set;}
		public Vector3 LocalFrontBottomRight {get; private set;}
		public Vector3 LocalBackTopLeft => -LocalFrontBottomRight;
		public Vector3 LocalBackTopRight => -LocalFrontBottomLeft;
		public Vector3 LocalBackBottomLeft => -LocalFrontTopRight;
		public Vector3 LocalBackBottomRight => -LocalFrontTopLeft;

		public Vector3 FrontTopLeft => LocalFrontTopLeft + origin;
		public Vector3 FrontTopRight => LocalFrontTopRight + origin;
		public Vector3 FrontBottomLeft => LocalFrontBottomLeft + origin;
		public Vector3 FrontBottomRight => LocalFrontBottomRight + origin;
		public Vector3 BackTopLeft => LocalBackTopLeft + origin;
		public Vector3 BackTopRight => LocalBackTopRight + origin;
		public Vector3 BackBottomLeft => LocalBackBottomLeft + origin;
		public Vector3 BackBottomRight => LocalBackBottomRight + origin;

		public Vector3 origin {get; private set;}

		public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
		{
			Rotate(orientation);
		}
		public Box(Vector3 origin, Vector3 halfExtents)
		{
			this.LocalFrontTopLeft     = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
			this.LocalFrontTopRight    = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
			this.LocalFrontBottomLeft  = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
			this.LocalFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

			this.origin = origin;
		}


		public void Rotate(Quaternion orientation)
		{
			LocalFrontTopLeft     = RotatePointAroundPivot(LocalFrontTopLeft    , Vector3.zero, orientation);
			LocalFrontTopRight    = RotatePointAroundPivot(LocalFrontTopRight   , Vector3.zero, orientation);
			LocalFrontBottomLeft  = RotatePointAroundPivot(LocalFrontBottomLeft , Vector3.zero, orientation);
			LocalFrontBottomRight = RotatePointAroundPivot(LocalFrontBottomRight, Vector3.zero, orientation);
		}
		
		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
		{
			Vector3 direction = point - pivot;
			return pivot + rotation * direction;
		}
	}

}