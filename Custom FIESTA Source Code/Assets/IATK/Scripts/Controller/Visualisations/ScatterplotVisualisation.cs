﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;


namespace IATK
{
    [ExecuteInEditMode]
    public class ScatterplotVisualisation : AbstractVisualisation 
    {

        public override void CreateVisualisation()
        {
            string savedName = name;

            foreach (View v in viewList)
            {
                DestroyImmediate(v.gameObject);
            }

            viewList.Clear();

            // Create new
            Dictionary<CreationConfiguration.Axis, string> axies = new Dictionary<CreationConfiguration.Axis, string>();
            if (visualisationReference.xDimension.Attribute != "" && visualisationReference.xDimension.Attribute != "Undefined")
            {
                axies.Add(CreationConfiguration.Axis.X, visualisationReference.xDimension.Attribute);
            }
            if (visualisationReference.yDimension.Attribute != "" && visualisationReference.yDimension.Attribute != "Undefined")
            {
                axies.Add(CreationConfiguration.Axis.Y, visualisationReference.yDimension.Attribute);
            }
            if (visualisationReference.zDimension.Attribute != "" && visualisationReference.zDimension.Attribute != "Undefined")
            {
                axies.Add(CreationConfiguration.Axis.Z, visualisationReference.zDimension.Attribute);
            }

            if (creationConfiguration == null)
                creationConfiguration = new CreationConfiguration(visualisationReference.geometry, axies);
            else
            {
                creationConfiguration.Geometry = visualisationReference.geometry;
                creationConfiguration.Axies = axies;
                creationConfiguration.LinkingDimension = visualisationReference.linkingDimension;
                creationConfiguration.Size = visualisationReference.size;
                creationConfiguration.MinSize = visualisationReference.minSize;
                creationConfiguration.MaxSize = visualisationReference.maxSize;
            }

            View view = CreateSimpleVisualisation(creationConfiguration);

            if (view != null)
            {
                view.transform.localPosition = Vector3.zero;
                view.transform.SetParent(transform, false);
                view.onViewChangeEvent += UpdateVisualisation;   // Receive notifications when the view configuration changes

                viewList.Add(view);
            }

            if (viewList.Count > 0 && visualisationReference.colourDimension != "Undefined")
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    viewList[i].SetColors(mapColoursContinuous(visualisationReference.dataSource[visualisationReference.colourDimension].Data));
                }
            }
            else if (viewList.Count > 0 && visualisationReference.colorPaletteDimension != "Undefined")
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    viewList[i].SetColors(mapColoursPalette(visualisationReference.dataSource[visualisationReference.colorPaletteDimension].Data, visualisationReference.coloursPalette));
                }
            }
            else if (viewList.Count > 0 && visualisationReference.colour != null)
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    Color[] colours = viewList[i].GetColors();
                    for (int j = 0; j < colours.Length; ++j)
                    {
                        colours[j] = visualisationReference.colour;
                    }
                    viewList[i].SetColors(colours);
                }
            }


            if (viewList.Count > 0)
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    viewList[i].SetSize(visualisationReference.size);
                    viewList[i].SetMinSize(visualisationReference.minSize);
                    viewList[i].SetMaxSize(visualisationReference.maxSize);

                    viewList[i].SetMinNormX(visualisationReference.xDimension.minScale);
                    viewList[i].SetMaxNormX(visualisationReference.xDimension.maxScale);
                    viewList[i].SetMinNormY(visualisationReference.yDimension.minScale);
                    viewList[i].SetMaxNormY(visualisationReference.yDimension.maxScale);
                    viewList[i].SetMinNormZ(visualisationReference.zDimension.minScale);
                    viewList[i].SetMaxNormZ(visualisationReference.zDimension.maxScale);

                    viewList[i].SetMinX(visualisationReference.xDimension.minFilter);
                    viewList[i].SetMaxX(visualisationReference.xDimension.maxFilter);
                    viewList[i].SetMinY(visualisationReference.yDimension.minFilter);
                    viewList[i].SetMaxY(visualisationReference.yDimension.maxFilter);
                    viewList[i].SetMinZ(visualisationReference.zDimension.minFilter);
                    viewList[i].SetMaxZ(visualisationReference.zDimension.maxFilter);
                }
            }

            if (viewList.Count > 0 && visualisationReference.sizeDimension != "Undefined")
            {
                for (int i = 0; i < viewList.Count; i++)
                {
                    viewList[i].SetSizeChannel(visualisationReference.dataSource[visualisationReference.sizeDimension].Data);
                }
            }

            name = savedName;
        }

        public override void UpdateVisualisation(PropertyType propertyType){

            if (viewList.Count == 0)
                CreateVisualisation();

            if (viewList.Count != 0)
                switch (propertyType)
                {
                    case AbstractVisualisation.PropertyType.X:
                        if (visualisationReference.xDimension.Attribute.Equals("Undefined"))
                        {
                            viewList[0].ZeroPosition(0);
                            viewList[0].TweenPosition();
                        }
                        else
                        {
                            viewList[0].UpdateXPositions(visualisationReference.dataSource[visualisationReference.xDimension.Attribute].Data);
                            viewList[0].TweenPosition();
                        }
                        if (creationConfiguration.Axies.ContainsKey(CreationConfiguration.Axis.X))
                        {
                            creationConfiguration.Axies[CreationConfiguration.Axis.X] = visualisationReference.xDimension.Attribute;
                        }
                        else
                        {
                            creationConfiguration.Axies.Add(CreationConfiguration.Axis.X, visualisationReference.xDimension.Attribute);
                        }
                        break;
                    case AbstractVisualisation.PropertyType.Y:
                        if (visualisationReference.yDimension.Attribute.Equals("Undefined"))
                        {
                            viewList[0].ZeroPosition(1);
                            viewList[0].TweenPosition();
                        }
                        else
                        {
                            viewList[0].UpdateYPositions(visualisationReference.dataSource[visualisationReference.yDimension.Attribute].Data);
                            viewList[0].TweenPosition();
                        }
                        if (creationConfiguration.Axies.ContainsKey(CreationConfiguration.Axis.Y))
                        {
                            creationConfiguration.Axies[CreationConfiguration.Axis.Y] = visualisationReference.yDimension.Attribute;
                        }
                        else
                        {
                            creationConfiguration.Axies.Add(CreationConfiguration.Axis.Y, visualisationReference.yDimension.Attribute);
                        }
                        break;
                    case AbstractVisualisation.PropertyType.Z:
                        if (visualisationReference.zDimension.Attribute.Equals("Undefined"))
                        {
                            viewList[0].ZeroPosition(2);
                            viewList[0].TweenPosition();
                        }
                        else
                        {
                            viewList[0].UpdateZPositions(visualisationReference.dataSource[visualisationReference.zDimension.Attribute].Data);
                            viewList[0].TweenPosition();
                        }
                        if (creationConfiguration.Axies.ContainsKey(CreationConfiguration.Axis.Z))
                        {
                            creationConfiguration.Axies[CreationConfiguration.Axis.Z] = visualisationReference.zDimension.Attribute;
                        }
                        else
                        {
                            creationConfiguration.Axies.Add(CreationConfiguration.Axis.Z, visualisationReference.zDimension.Attribute);
                        }
                        break;
                    case AbstractVisualisation.PropertyType.Colour:
                        if (visualisationReference.colourDimension != "Undefined")
                        {
                            for (int i = 0; i < viewList.Count; i++)
                                viewList[i].SetColors(mapColoursContinuous(visualisationReference.dataSource[visualisationReference.colourDimension].Data));
                        }
                        else if (visualisationReference.colorPaletteDimension != "Undefined")
                        {
                            for (int i = 0; i < viewList.Count; i++)
                            {
                                viewList[i].SetColors(mapColoursPalette(visualisationReference.dataSource[visualisationReference.colorPaletteDimension].Data, visualisationReference.coloursPalette));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < viewList.Count; i++)
                            {
                                Color[] colours = viewList[0].GetColors();
                                for (int j = 0; j < colours.Length; ++j)
                                {
                                    colours[j] = visualisationReference.colour;
                                }
                                viewList[i].SetColors(colours);
                            }

                        }

                        creationConfiguration.ColourDimension = visualisationReference.colourDimension;
                        creationConfiguration.colourKeys = visualisationReference.dimensionColour;
                        creationConfiguration.colour = visualisationReference.colour;

                        break;
                    case AbstractVisualisation.PropertyType.Size:
                        {
                            for (int i = 0; i < viewList.Count; i++)
                            {
                                if (visualisationReference.sizeDimension != "Undefined")
                                {
                                    viewList[i].SetSizeChannel(visualisationReference.dataSource[visualisationReference.sizeDimension].Data);
                                }
                                else
                                {
                                    viewList[i].SetSizeChannel(Enumerable.Repeat(0f, visualisationReference.dataSource[0].Data.Length).ToArray());
                                }
                            }
                            creationConfiguration.SizeDimension = visualisationReference.sizeDimension;       
                            viewList[0].TweenSize();

                            break;

                        }
                    case PropertyType.SizeValues:
                        for (int i = 0; i < viewList.Count; i++)
                        {
                            viewList[i].SetSize(visualisationReference.size);
                            viewList[i].SetMinSize(visualisationReference.minSize);        // Data is normalised
                            viewList[i].SetMaxSize(visualisationReference.maxSize);
                        }
                        creationConfiguration.Size = visualisationReference.size;
                        creationConfiguration.MinSize = visualisationReference.minSize;
                        creationConfiguration.MaxSize = visualisationReference.maxSize;

                        break;
                    case AbstractVisualisation.PropertyType.LinkingDimension:
                        creationConfiguration.LinkingDimension = visualisationReference.linkingDimension;

                        string xDimension = visualisationReference.xDimension.Attribute;
                        string yDimension = visualisationReference.yDimension.Attribute;
                        string linkingDimension = visualisationReference.linkingDimension;
                        float[] newAggregatedYValues = new float[0];

                        if (xDimension != "Undefined" && yDimension != "Undefined" && linkingDimension != "Undefined")
                        {
                            float[] xData = visualisationReference.dataSource[xDimension].Data;
                            float[] yData = visualisationReference.dataSource[yDimension].Data;
                            float[] linkingData = visualisationReference.dataSource[linkingDimension].Data;

                            // Create new list of pairs where Item1 is the x dimension, Item2 is the linking dimension
                            List<Tuple<float, float>> pairs = new List<Tuple<float, float>>();
                            for (int i = 0; i < visualisationReference.dataSource.DataCount; i++)
                            {
                                pairs.Add(new Tuple<float, float>(xData[i], linkingData[i]));
                            }

                            // Sort by x dimension values, then by linking values
                            var sort = pairs.Select((value, index) => new { value, index }).OrderBy(x => x.value.Item1).ThenBy(x => x.value.Item2).ToList();
                            //List<float> sortedXValues = sort.Select(x => x.value.Item1).ToList();
                            //List<float> sortedLinkingValues = sort.Select(x => x.value.Item2).ToList();
                            //List<int> sortedIndices = sort.Select(x => x.index).ToList();

                            List<int> associatedIndices = new List<int>();

                            newAggregatedYValues = new float[visualisationReference.dataSource.DataCount];
                            
                            int count = 0;
                            double total = 0;
                            float previousX = Mathf.Infinity;
                            float previousLinking = Mathf.Infinity;
                            float avg;

                            for (int i = 0; i < sort.Count; i++)
                            {
                                float currentX = sort[i].value.Item1;
                                float currentLinking = sort[i].value.Item2;
                                
                                // If now looking at a new x, linking group
                                if (previousLinking != currentLinking || previousX != currentX)
                                {
                                    // Calculate average
                                    avg = (float)(total / count);

                                    // Back-fill the values of the averages to the associated indices
                                    foreach (int idx in associatedIndices)
                                    {
                                        newAggregatedYValues[idx] = avg;
                                    }

                                    associatedIndices.Clear();
                                    count = 0;
                                    total = 0;
                                }

                                previousX = currentX;
                                previousLinking = currentLinking;

                                int actualIndex = sort[i].index;

                                total += yData[actualIndex];
                                count++;

                                associatedIndices.Add(actualIndex);
                            }

                            // Do one last final check
                            avg = (float)(total / count);

                            // Back-fill the values of the averages to the associated indices
                            foreach (int idx in associatedIndices)
                            {
                                newAggregatedYValues[idx] = avg;
                            }
                        }
                        
                        CreateVisualisation(); // needs to recreate the visualsiation because the mesh properties have changed 

                        if (newAggregatedYValues.Length > 0)
                        {
                            viewList[0].UpdateYPositions(newAggregatedYValues);
                        }
                        break;

                    case AbstractVisualisation.PropertyType.GeometryType:
                        creationConfiguration.Geometry = visualisationReference.geometry;
                        CreateVisualisation(); // needs to recreate the visualsiation because the mesh properties have changed 
                        break;

                    case AbstractVisualisation.PropertyType.Scaling:
                        
                        for (int i = 0; i < viewList.Count; i++)
                        {
                            viewList[i].SetMinNormX(visualisationReference.xDimension.minScale);
                            viewList[i].SetMaxNormX(visualisationReference.xDimension.maxScale);
                            viewList[i].SetMinNormY(visualisationReference.yDimension.minScale);
                            viewList[i].SetMaxNormY(visualisationReference.yDimension.maxScale);
                            viewList[i].SetMinNormZ(visualisationReference.zDimension.minScale);
                            viewList[i].SetMaxNormZ(visualisationReference.zDimension.maxScale);
                        }
                        break;

                    case AbstractVisualisation.PropertyType.DimensionFiltering:
                        for (int i = 0; i < viewList.Count; i++)
                        {
                            viewList[i].SetMinX(visualisationReference.xDimension.minFilter);
                            viewList[i].SetMaxX(visualisationReference.xDimension.maxFilter);
                            viewList[i].SetMinY(visualisationReference.yDimension.minFilter);
                            viewList[i].SetMaxY(visualisationReference.yDimension.maxFilter);
                            viewList[i].SetMinZ(visualisationReference.zDimension.minFilter);
                            viewList[i].SetMaxZ(visualisationReference.zDimension.maxFilter);
                        }
                        break;
                    case AbstractVisualisation.PropertyType.AttributeFiltering:
                        if (visualisationReference.attributeFilters!=null)
                        {
                            foreach (var viewElement in viewList)
                            {
                                float[] isFiltered = new float[visualisationReference.dataSource.DataCount];
                                for (int i = 0; i < visualisationReference.dataSource.DimensionCount; i++)
                                {
                                    foreach (AttributeFilter attrFilter in visualisationReference.attributeFilters)
                                    {
                                        if (attrFilter.Attribute == visualisationReference.dataSource[i].Identifier)
                                        {
                                            float minFilteringValue = UtilMath.normaliseValue(attrFilter.minFilter, 0f, 1f, attrFilter.minScale, attrFilter.maxScale);
                                            float maxFilteringValue = UtilMath.normaliseValue(attrFilter.maxFilter, 0f, 1f, attrFilter.minScale, attrFilter.maxScale);

                                            for (int j = 0; j < isFiltered.Length; j++)
                                            {
                                                isFiltered[j] = (visualisationReference.dataSource[i].Data[j] < minFilteringValue || visualisationReference.dataSource[i].Data[j] > maxFilteringValue) ? 1.0f : isFiltered[j];
                                            }
                                        }
                                    }
                                }
                                // map the filtered attribute into the normal channel of the bigmesh
                                foreach (View v in viewList)
                                {
                                    v.SetFilterChannel(isFiltered);
                                }
                            }
                        }
                        break;
                    case PropertyType.VisualisationType:                       
                        break;
                    case PropertyType.BlendDestinationMode:
                        float bmds = (int)(System.Enum.Parse(typeof(UnityEngine.Rendering.BlendMode), visualisationReference.blendingModeDestination));
                        for (int i = 0; i < viewList.Count; i++)
                        {
                            viewList[i].SetBlendindDestinationMode(bmds);
                        }

                            break;
                    case PropertyType.BlendSourceMode:
                        float bmd = (int)(System.Enum.Parse(typeof(UnityEngine.Rendering.BlendMode), visualisationReference.blendingModeSource));
                        for (int i = 0; i < viewList.Count; i++)
                        {
                            viewList[i].SetBlendingSourceMode(bmd);
                        }

                        break;
                    default:
                        break;
                }

            //if (visualisationReference.geometry != GeometryType.Undefined)// || visualisationType == VisualisationTypes.PARALLEL_COORDINATES)
            //    SerializeViewConfiguration(creationConfiguration);

            //Update any label on the corresponding axes
            UpdateVisualisationAxes(propertyType);
        }

        protected void UpdateVisualisationAxes(AbstractVisualisation.PropertyType propertyType)
        {
            switch (propertyType)
            {
                case AbstractVisualisation.PropertyType.X:
                    if (visualisationReference.xDimension.Attribute == "Undefined" && X_AXIS != null)// GameObject_Axes_Holders[0] != null)
                    {
                        DestroyImmediate(X_AXIS);
                    }
                    else if (X_AXIS != null)
                    {
                        Axis a = X_AXIS.GetComponent<Axis>();
                        a.Init(visualisationReference.dataSource, visualisationReference.xDimension, visualisationReference);
                        BindMinMaxAxisValues(a, visualisationReference.xDimension);
                    }
                    else if (visualisationReference.xDimension.Attribute != "Undefined")
                    {
                        Vector3 pos = Vector3.zero;
                        pos.y = -0.02f;
                        X_AXIS = CreateAxis(propertyType, visualisationReference.xDimension, pos, new Vector3(0f, 0f, 0f), 0);   
                        
                    }
                    break;
                case AbstractVisualisation.PropertyType.Y:
                    if (visualisationReference.yDimension.Attribute == "Undefined" && Y_AXIS != null)
                    {
                        DestroyImmediate(Y_AXIS);
                    }
                    else if (Y_AXIS != null)
                    {
                        Axis a = Y_AXIS.GetComponent<Axis>();
                        a.Init(visualisationReference.dataSource, visualisationReference.yDimension, visualisationReference);
                        BindMinMaxAxisValues(a, visualisationReference.yDimension);
                    }
                    else if (visualisationReference.yDimension.Attribute != "Undefined")
                    {
                        Vector3 pos = Vector3.zero;
                        pos.x = -0.02f;
                        Y_AXIS = CreateAxis(propertyType, visualisationReference.yDimension, pos, new Vector3(0f, 0f, 0f), 1);
                    }
                    break;
                case AbstractVisualisation.PropertyType.Z:
                    if (visualisationReference.zDimension.Attribute == "Undefined" && Z_AXIS != null)
                    {
                        DestroyImmediate(Z_AXIS);
                    }
                    else if (Z_AXIS != null)
                    {
                        Axis a = Z_AXIS.GetComponent<Axis>();
                        a.Init(visualisationReference.dataSource, visualisationReference.zDimension, visualisationReference);
                        BindMinMaxAxisValues(Z_AXIS.GetComponent<Axis>(), visualisationReference.zDimension);
                    }
                    else if (visualisationReference.zDimension.Attribute != "Undefined")
                    {
                        Vector3 pos = Vector3.zero;
                        pos.y = -0.02f;
                        pos.x = -0.02f;
                        Z_AXIS = CreateAxis(propertyType, visualisationReference.zDimension, pos, new Vector3(90f, 0f, 0f), 2);
                    }
                    break;

                case AbstractVisualisation.PropertyType.DimensionFiltering:
                    if (visualisationReference.xDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(X_AXIS.GetComponent<Axis>(), visualisationReference.xDimension);
                    }
                    if (visualisationReference.yDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(Y_AXIS.GetComponent<Axis>(), visualisationReference.yDimension);
                    }
                    if (visualisationReference.zDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(Z_AXIS.GetComponent<Axis>(), visualisationReference.zDimension);
                    }
                    break;
                case AbstractVisualisation.PropertyType.Scaling:
                    if (visualisationReference.xDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(X_AXIS.GetComponent<Axis>(), visualisationReference.xDimension);
                    }
                    if (visualisationReference.yDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(Y_AXIS.GetComponent<Axis>(), visualisationReference.yDimension);
                    }
                    if (visualisationReference.zDimension.Attribute != "Undefined")
                    {
                        BindMinMaxAxisValues(Z_AXIS.GetComponent<Axis>(), visualisationReference.zDimension);
                    }
                    break;
                default:
                    break;
            }

            UpdateAxes();
        }
        
        /// <summary>
        /// Gets the axies.
        /// </summary>
        /// <returns>The axies.</returns>
        /// <param name="axies">Axies.</param>
        protected string getAxis(Dictionary<CreationConfiguration.Axis, string> axies, CreationConfiguration.Axis axis)
        {

            string axes = null;
            string retVal = "";
            if (axies.TryGetValue(axis, out axes))
                retVal = axes;

            return retVal;
        }

        public override void SaveConfiguration(){}

        public override void LoadConfiguration(){}
        
        /// <summary>
        /// Maps the colours.
        /// </summary>
        /// <returns>The colours.</returns>
        /// <param name="data">Data.</param>
        public override Color[] mapColoursContinuous(float[] data)
        {
            Color[] colours = new Color[data.Length];

            for (int i = 0; i < data.Length; ++i)
            {
                colours[i] = visualisationReference.dimensionColour.Evaluate(data[i]);
            }

            return colours;
        }

        public Color[] mapColoursPalette(float[] data, Color[] palette)
        {
            Color[] colours = new Color[data.Length];

            float[] uniqueValues = data.Distinct().ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                int indexColor = Array.IndexOf(uniqueValues, data[i]);
                colours[i] = palette[indexColor];
            }

            return colours;
        }

        // ******************************
        // SPECIFIC VISUALISATION METHODS
        // ******************************
        
        private View CreateSimpleVisualisation(CreationConfiguration configuration)
        {

            if (visualisationReference.dataSource != null)
            {
                if (!visualisationReference.dataSource.IsLoaded) visualisationReference.dataSource.load();

                ViewBuilder builder = new ViewBuilder(geometryToMeshTopology(configuration.Geometry), "Simple Visualisation");

                if ((visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.X)] != null) ||
                    (visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Y)] != null) ||
                    (visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Z)] != null))
                {
                    builder.initialiseDataView(visualisationReference.dataSource.DataCount);

                    if (visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.X)] != null)
                        builder.setDataDimension(visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.X)].Data, ViewBuilder.VIEW_DIMENSION.X);
                    if (visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Y)] != null)
                        builder.setDataDimension(visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Y)].Data, ViewBuilder.VIEW_DIMENSION.Y);
                    if (visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Z)] != null)
                        builder.setDataDimension(visualisationReference.dataSource[getAxis(configuration.Axies, CreationConfiguration.Axis.Z)].Data, ViewBuilder.VIEW_DIMENSION.Z);

                    //destroy the view to clean the big mesh
                    destroyView();

                    //return the appropriate geometry view
                    return ApplyGeometryAndRendering(creationConfiguration, ref builder);
                }

            }

            return null;

        }

        // *************************************************************
        // ********************  UNITY METHODS  ************************
        // *************************************************************

        // Use this for initialization
        
        private void Update()
        {
            //UpdateAxes();
        }

        private void UpdateAxes()
        {
            // handle update to persistent properties
            if (X_AXIS != null && X_AXIS.activeSelf)
            {
                X_AXIS.GetComponent<Axis>().Length = visualisationReference.width;
                X_AXIS.GetComponent<Axis>().UpdateLength();
            }
            if (Y_AXIS != null && Y_AXIS.activeSelf)
            {
                Y_AXIS.GetComponent<Axis>().Length = visualisationReference.height;
                Y_AXIS.GetComponent<Axis>().UpdateLength();
            }
            if (Z_AXIS != null && Z_AXIS.activeSelf)
            {
                Z_AXIS.GetComponent<Axis>().Length = visualisationReference.depth;
                Z_AXIS.GetComponent<Axis>().UpdateLength();
            }

            foreach (View view in viewList)
            {
                view.transform.localScale = new Vector3(
                    visualisationReference.width,
                    visualisationReference.height,
                    visualisationReference.depth
                );
            }
        }
    }
}
