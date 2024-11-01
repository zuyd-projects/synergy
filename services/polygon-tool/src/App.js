import React, { useEffect, useRef, useState } from "react";
import mapboxgl from "mapbox-gl";
import MapboxDraw from "@mapbox/mapbox-gl-draw";

import "mapbox-gl/dist/mapbox-gl.css";
import "@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css";

const paragraphStyle = {
  fontFamily: "Open Sans",
  margin: 0,
  fontSize: 13,
};

const App = () => {
  const mapContainerRef = useRef();
  const mapRef = useRef();
  const [polygonPointsString, setPolygonPointsString] = useState(""); // State to hold formatted string

  useEffect(() => {
    mapboxgl.accessToken = `${process.env.REACT_APP_MAPBOX_TOKEN}`;


    // Extract lat and lng from URL parameters, with fallback values if not found
    const urlParams = new URLSearchParams(window.location.search);
    const lat = parseFloat(urlParams.get("lat")) || 50.9282385;
    const lng = parseFloat(urlParams.get("lng")) || 5.9993508;

    // Initialize the map with the center from URL parameters or default values
    mapRef.current = new mapboxgl.Map({
      container: mapContainerRef.current,
      style: "mapbox://styles/mapbox/satellite-v9",
      center: [lng, lat], // Use URL parameters for center if available
      zoom: 12,
    });

    const draw = new MapboxDraw({
      displayControlsDefault: false,
      controls: {
        polygon: true,
        trash: true,
      },
      defaultMode: "draw_polygon",
    });
    mapRef.current.addControl(draw);

    mapRef.current.on("draw.create", updateArea);
    mapRef.current.on("draw.delete", updateArea);
    mapRef.current.on("draw.update", updateArea);

    function updateArea(e) {
      const data = draw.getAll();
      if (data.features.length > 0) {
        const coordinates = data.features[0].geometry.coordinates[0]; // Assuming a single polygon

        // Format points as "POINT(lat lng)"
        const formattedPoints = coordinates.map(
          ([lng, lat]) => `POINT(${lat} ${lng})`
        );

        // Join all points into a single string
        const pointsString = formattedPoints.join(", ");
        setPolygonPointsString(pointsString); // Update state with formatted string
      } else {
        setPolygonPointsString("");
        if (e.type !== "draw.delete") alert("Click the map to draw a polygon.");
      }
    }
  }, []);

  return (
    <>
      <div ref={mapContainerRef} id="map" style={{ height: "100%" }}></div>
      <div
        className="calculation-box"
        style={{
          height: "auto",
          width: 300,
          position: "absolute",
          bottom: 40,
          left: 10,
          backgroundColor: "rgba(255, 255, 255, 0.9)",
          padding: 15,
          textAlign: "left",
        }}
      >
        <p style={paragraphStyle}>
          Made by <a href="https://nahnova.com">Noa Heutz</a>
        </p>
        <p style={paragraphStyle}>Click the map to draw a polygon.</p>
        <div id="polygon-points">
          <p style={paragraphStyle}>
            <strong>PolygonPoints</strong>
          </p>
          <p style={paragraphStyle}>{polygonPointsString}</p>
        </div>
      </div>
    </>
  );
};

export default App;
