      // create some xml and write it to file
      var root = new XML("<root/>");
      var child = new XML("<child/>");
      child.@string = "Hello Attribute"; // jshint ignore:line
      child.@num = 23; // jshint ignore:line
      root.appendChild(child);
      var file = new File("~/Desktop/test.xml");
      var xml = root.toXMLString();
      file.open("W");
      file.write(xml);
      file.close();