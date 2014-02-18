﻿using System;
using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public class FieldManipulatorProperties
    {
        public double AdjustmentSize { get; private set; }
        public int AdjustmentRadius { get; private set; }

        private readonly IFieldManipulatorOptions _options;

        public FieldManipulatorProperties(IFieldManipulatorOptions options)
        {
            _options = options;

            AdjustmentSize = 0.1;
            AdjustmentRadius = 1;
        }

        public void Update()
        {
            UpdateIntensity();
            UpdateRadius();
        }

        private void UpdateIntensity()
        {
            if (Input.GetKeyDown(_options.IntensityIncreaseKey))
            {
                AdjustmentSize = 10 * AdjustmentSize;
            }
            else if (Input.GetKeyDown(_options.IntensityDecreaseKey))
            {
                AdjustmentSize = 0.1 * AdjustmentSize;
            }
        }

        private void UpdateRadius()
        {
            if (Input.GetKeyDown(_options.RadiusIncreaseKey))
            {
                AdjustmentRadius = AdjustmentRadius + 1;
            }
            else if (Input.GetKeyDown(_options.RadiusDecreaseKey))
            {
                AdjustmentRadius = AdjustmentRadius - 1;
            }
        }

        public void UpdateGUI()
        {
            var labelText = String.Format("Adjustment Size: {0:F0}m\nRadius: {1:N} cells", 1000*AdjustmentSize, AdjustmentRadius);
            GUI.Label(new Rect(10, Screen.height - 50, 200, 40), labelText);
        }
    }
}
