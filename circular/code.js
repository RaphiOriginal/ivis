var div = document.getElementById("#chart");

var width = 960
var height = 500
var radius = Math.min(width, height) / 2;

var arc = d3.svg.arc()
    .outerRadius(radius - 10)
    .innerRadius(radius - 70);

var arc2 = d3.svg.arc()
    .outerRadius(radius - 70)
    .innerRadius(radius - 140);

var arc3 = d3.svg.arc()
    .outerRadius(radius - 140)
    .innerRadius(radius - 210);


var svg = d3.select("body").append("svg")
    .attr("width", width)
    .attr("height", height)
    .append("g")
    .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

//SELECT COUNT(*) AS chs FROM ivis_fs16.Ort WHERE AUTOZEICHEN = 'CH'UNION SELECT COUNT(*) AS total FROM ivis_fs16.Ort;
d3.json("data1.json", function(error, data){
  if(error) throw error;

  arc.startAngle(0);
  arc.endAngle(toRadiant(data[0].chs, data[1].chs));
  svg.append("path")
    .attr("class", "arc")
    .attr("d", arc);

  svg.append("text")
    .attr("transform", function(d) { return "translate(" + arc.centroid(d) + ")"; })
    .attr("text-anchor", "middle")
    .text("🇨🇭");
});

//SELECT COUNT(*) AS dead FROM ivis_fs16.Person WHERE TODJAHR != "" UNION SELECT COUNT(*) FROM ivis_fs16.Person;
d3.json("data2.json", function(error, data){
  if(error) throw error;

  arc2.startAngle(0);
  arc2.endAngle(toRadiant(data[0].dead, data[1].dead));
  svg.append("path")
    .attr("class", "arc2")
    .attr("d", arc2);

  svg.append("text")
    .attr("transform", function(d) { return "translate(" + arc2.centroid(d) + ")"; })
    .attr("text-anchor", "middle")
    .text("💀");
});

//SELECT COUNT(*) AS who FROM ivis_fs16.Werk WHERE BESITZER_RECHTE = 'UNGEKLÄRT' UNION SELECT COUNT(*) FROM ivis_fs16.Werk;
d3.json("data3.json", function(error, data){
  if(error) throw error;

  arc3.startAngle(0);
  arc3.endAngle(toRadiant(data[0].who, data[1].who));
  svg.append("path")
    .attr("class", "arc3")
    .attr("d", arc3);

  svg.append("text")
    .attr("transform", function(d) { return "translate(" + arc3.centroid(d) + ")"; })
    .attr("text-anchor", "middle")
    .text("👤");
});

function toRadiant(value, max){
  var smallest = (2 * Math.PI) / max;
  return value * smallest;
}
