var cy = cytoscape({
  container: document.getElementById('cy'),
  elements: {
    nodes: [
      { data: { id: '0'} },
      { data: { id: '1'} },
    ],
    edges: [
      { data: { source: '0', target: '1' } },
] },
});