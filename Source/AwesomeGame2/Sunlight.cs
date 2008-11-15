using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2
{
    static class Sunlight
    {
        public static void ApplyToBasicEffect(BasicEffect effect)
        {
            effect.EnableDefaultLighting();

            effect.DirectionalLight0.Direction = new Vector3(1.0f, 0.0f, 0.0f);
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.95f);

            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight2.Enabled = false;

            effect.AmbientLightColor = new Vector3(0.05f);
        }
    }
}
