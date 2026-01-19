namespace ATAS.Indicators.Custom;

using ATAS.Indicators;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

using Utils.Common.Collections;

[Category(IndicatorCategories.Custom)]
[DisplayName("Maximum Level on candles")]
[Display(Description = "Displays the price corresponding to the maximum level volume on each candle.")]
public class MaximumLevelOnCandles  : Indicator
{
    private readonly PriceSelectionDataSeries _priceSelectionSeries = new PriceSelectionDataSeries("MaximumLevel");
    private Color _pocLevelColor;
    private Color _objectColor;
    private ObjectType _visualType = ObjectType.Rectangle;
    private int _objectSize;

    [Display(Name = "Maximum Level Price Color", Description = "Color of price corresponding to the maximum level.", GroupName = "Visualization")]
    public Color PocLevelColor
    {
        get => _pocLevelColor;
        set
        {
            _pocLevelColor = value;

            for (var i = 0; i < _priceSelectionSeries.Count; i++)
            {
                _priceSelectionSeries[i].ForEach(x => { x.ObjectColor = _pocLevelColor; });
            }

            RecalculateValues();
        }
    }

    [Display(Name = "Visual Object Type", Description = "Type of visual object to represent the maximum level.", GroupName = "Visualization")]
    public ObjectType ObjectType
    {
        get => _visualType;
        set
        {
            _visualType = value;
            for (var i = 0; i < _priceSelectionSeries.Count; i++)
            {
                _priceSelectionSeries[i].ForEach(x => { x.VisualObject = _visualType; });
            }
        }
    }

    [Display(Name = "Visual Object Color", Description = "Color of visual object to represent the maximum level.", GroupName = "Visualization")]
    public Color ObjectColor
    {
        get => _objectColor;
        set
        {
            _objectColor = value;
            for (var i = 0; i < _priceSelectionSeries.Count; i++)
            {
                _priceSelectionSeries[i].ForEach(x => { x.ObjectColor = _objectColor; });
            }
        }
    }

    [Display(Name = "Visual Object Size", Description = "Size of visual object to represent the maximum level.", GroupName = "Visualization")]
    [Range(1, 20)]
    public int ObjectSize
    {
        get => _objectSize;
        set
        {
            _objectSize = value;
            for (var i = 0; i < _priceSelectionSeries.Count; i++)
            {
                _priceSelectionSeries[i].ForEach(x => { x.Size = _objectSize; });
            }
        }
    }

    public MaximumLevelOnCandles()
    {
        DenyToChangePanel = true;
        _priceSelectionSeries.IsHidden = true;

        _priceSelectionSeries!.Name = "Maximum Level Price";
        PocLevelColor = Color.FromArgb(120, 240, 164, 189);
        ObjectType = ObjectType.Rectangle;
        ObjectColor = Color.FromArgb(120, 240, 164, 189);
        ObjectSize = 5;

        DataSeries[0] = _priceSelectionSeries;
    }

    protected override void OnCalculate(int bar, decimal value)
    {
        var candle = GetCandle(bar);
        
        _priceSelectionSeries[bar].Clear();
        _priceSelectionSeries[bar].Add(new PriceSelectionValue(candle.MaxVolumePriceInfo?.Price ?? 0m)
        {
            PriceSelectionColor = PocLevelColor,
            VisualObject = ObjectType,
            ObjectColor = ObjectColor,
            Size = ObjectSize,
        });
    }
}
