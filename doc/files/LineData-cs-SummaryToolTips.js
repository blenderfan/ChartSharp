NDSummary.OnToolTipsLoaded("File:LineData.cs",{83:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype83\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\"><span class=\"Qualifier\">ChartSharp.</span>&#8203;LineData</div></div></div><div class=\"TTSummary\">This class is used to represent the data displayed in a LineChart. The data is handed over to a LineChart via XAML. You can add data to a chart in code by adding an instance of this class to the property Items of an instance of LineChart.</div></div>",86:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype86\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> LineData()</div><div class=\"TTSummary\">Initializes all properties to their default values.</div></div>",87:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype87\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> PresentationStyle DrawStyle { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">Affects the form of the rendering of the line.&nbsp; Possible values are:</div></div>",110:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype110\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> Brush Fill { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">A Brush, used to fill rendered shapes. If DrawStyle is either Point or Diamond, this property is used as the Brush to fill out those shapes. If it is either Lines or DashedLines, it is used to try to fill a Polygon spanned by the internally created Polyline.</div></div>",89:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype89\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> Brush Outline { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">A Brush, used to render the outline of shapes. If DrawStyle is either Point or Diamond, this property is used as the Brush for the outline of those shapes. If it is either Lines or DashedLines, it is used as the Brush for the created Lines.</div></div>",90:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype90\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public double</span> OutlineThickness { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">This property is only used if DrawStyle is either Point or Diamond. As the name suggests, this value specifies the thickness of the outline of shapes.</div></div>",93:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype93\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> PointCollection Points { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">The collection of Points used for a LineChart. The collection is not required to be sorted in any way. However, it should be noted that it will be sorted by X-Values internally before it is used.</div></div>",92:"<div class=\"NDToolTip TProperty LCSharp\"><div id=\"NDPrototype92\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public double</span> Thickness { <span class=\"SHKeyword\">get</span>; <span class=\"SHKeyword\">set</span> }</div><div class=\"TTSummary\">Specifies the thickness of lines or shapes.</div></div>"});