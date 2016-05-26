var cy = cytoscape({
  container: document.getElementById('cy'),
    
  style: [
           {
             selector: 'node',
             style: {
               'background-color': '#666',
               'label': 'data(name)'
             }
      },
           {
             selector: 'edge',
             style: {      
            	 'width': 3,
            	 'target-arrow-color': '#ccc',
            	 'target-arrow-shape': 'triangle'
             }
      } 
  ],
});


var req = new XMLHttpRequest();
req.addEventListener("load", reqListener);
req.open("GET", "http://server1102.cs.technik.fhnw.ch/json.php?t=Werk&n=1000&c=TITEL,JAHR,MATERIAL,BILDNAME");
req.send();

function reqListener () {
  var mats = new Set();
  var years = new Set();
  rows = JSON.parse(this.responseText);
  cy.startBatch();
  for(r in rows) {
	  obj = rows[r];
	  cy.add({group: "nodes", data: {id:r, name:obj['TITEL']}});
	  mat = obj['MATERIAL'];
	  if(!mats.has(mat)) {
		  mats.add(mat)
		  cy.add({group: "nodes", data: {id:mat, name:mat}});
	  }
    year = obj['JAHR'];
    if(!years.has(year)){
      years.add(year);
      cy.add({group: "nodes", data: {id:year, name:year}});
    }
	  cy.add({group: "edges", data: {source:r, target:mat}});
    cy.add({group: "edges", data: {source:r, target:year}}); 
  }
  cy.endBatch();
  cy.elements().layout({ name: 'cose' });
}
