using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector2 ToVector2(this Vector3 vector) {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector, float z = 0) {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector2 WithX(this Vector2 vector, float x) {
        return new Vector2(x, vector.y);
    }

    public static Vector2 WithY(this Vector2 vector, float y) {
        return new Vector2(vector.x, y);
    }

    public static Vector3 WithX(this Vector3 vector, float x) {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 WithY(this Vector3 vector, float y) {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 WithZ(this Vector3 vector, float z) {
        return new Vector3(vector.x, vector.y, z);
    }


    public static Vector3 Round(this Vector3 vector) => new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    public static Vector2 Round(this Vector2 vector) => new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));

    public static Vector3 Abs(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));

	public static Vector3 Max(Vector3 vector1, Vector3 vector2) => new Vector3(Mathf.Max(vector1.x, vector2.x), Mathf.Max(vector1.y, vector2.y), Mathf.Max(vector1.z, vector2.z));
	public static Vector3 Min(Vector3 vector1, Vector3 vector2) => new Vector3(Mathf.Min(vector1.x, vector2.x), Mathf.Min(vector1.y, vector2.y), Mathf.Min(vector1.z, vector2.z));

	public static Rect RectFromCenter(Vector2 center, Vector2 size) => new Rect(center - size / 2, size);
	public static Vector2 ClampInRect(Vector2 vector, Rect rect) => new Vector2(Mathf.Clamp(vector.x, rect.xMin, rect.xMax), Mathf.Clamp(vector.y, rect.yMin, rect.yMax));

	public static Vector2 ViewportSize(this Camera camera) => camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));


    public static Color WithAlpha(this Color color, float alpha) {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static float GetHue(this Color color) {
        float h;
        Color.RGBToHSV(color, out h, out _, out _);
        return h;
    }

    public static float GetSat(this Color color) {
        float s;
        Color.RGBToHSV(color, out _, out s, out _);
        return s;
    }

    public static float GetVal(this Color color) {
        float v;
        Color.RGBToHSV(color, out _, out _, out v);
        return v;
    }

    public static Color WithHue(this Color color, float hue) {
        float s, v;
        Color.RGBToHSV(color, out _, out s, out v);
        return Color.HSVToRGB(hue, s, v);
    }

    public static Color WithSat(this Color color, float sat) {
        float h, v;
        Color.RGBToHSV(color, out h, out _, out v);
        return Color.HSVToRGB(h, sat, v);
    }

    public static Color WithVal(this Color color, float val) {
        float h, s;
        Color.RGBToHSV(color, out h, out s, out _);
        return Color.HSVToRGB(h, s, val);
    }


    public static void SetColor(this ParticleSystem particleSystem, Color color) {
        var colorModule = particleSystem.colorOverLifetime;
        colorModule.enabled = true;

        // set color gradient to match color
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(color.WithAlpha(1), 0.0f),
                new GradientColorKey(color.WithAlpha(1), 1.0f)
            }, new GradientAlphaKey[] {
                new GradientAlphaKey(color.a, 0.0f),
                new GradientAlphaKey(color.a, 1.0f)
            }
        );

        colorModule.color = grad;
    }


    private static float VolumeToDB(float volume) => volume != 0 ? 40.0f * Mathf.Log10(volume) : -144.0f;
    private static float DBToVolume(float db) => Mathf.Pow(10.0f, db / 40.0f);
}

public delegate void EmptyDelegate();