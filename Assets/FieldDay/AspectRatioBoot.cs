using UnityEngine;
using FieldDay;
using FieldDay.Rendering;

public class AspectRatioBoot {
	[InvokePreBoot]
	static private void Configure() {
		Game.Rendering.EnableAspectClamping(1024, 768);
	}
}