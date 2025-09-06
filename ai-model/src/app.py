import os
import pickle
from flask import Flask, request, jsonify

app = Flask(__name__)

# Base path
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
MODEL_DIR = os.path.join(BASE_DIR, "../models")

# Load the model and label encoders
with open(os.path.join(MODEL_DIR, "tea_production_model.pkl"), "rb") as f:
    model = pickle.load(f)
with open(os.path.join(MODEL_DIR, "Label_Encode_FertilizerType.pkl"), "rb") as f:
    le_fertilizer = pickle.load(f)
with open(os.path.join(MODEL_DIR, "Label_Encode_Pruning.pkl"), "rb") as f:
    le_pruning = pickle.load(f)

@app.route("/predict", methods=["POST"])
def predict():
    try:
        # Get JSON input
        data = request.get_json()
        if not data:
            return jsonify({"error": "No data provided"}), 400

        # Extract all features
        try:
            year = int(data.get("Year"))
            month = int(data.get("Month"))
            plant_count = int(data.get("PlantCount"))
            fertilizer_type = data.get("FertilizerType")
            pruning = data.get("Pruning")
            soil_ph = float(data.get("SoilPH"))
            avg_rainfall = float(data.get("AvgRainfall"))
            avg_temp = float(data.get("AverageTemperature"))
            avg_humidity = float(data.get("AvgHumidityPercent"))
        except (TypeError, ValueError) as e:
            return jsonify({"error": f"Invalid or missing input: {str(e)}"}), 400

        # Encode categorical values
        try:
            fertilizer_transformed = le_fertilizer.transform([fertilizer_type])[0]
            pruning_transformed = le_pruning.transform([pruning])[0]
        except ValueError as e:
            return jsonify({"error": f"Invalid categorical input: {str(e)}"}), 400

        # Arrange input features in the correct order
        input_features = [[
            year,
            month,
            plant_count,
            fertilizer_transformed,
            pruning_transformed,
            soil_ph,
            avg_rainfall,
            avg_temp,
            avg_humidity
        ]]

        # Make prediction
        prediction = model.predict(input_features)

        return jsonify({"PredictionResult": float(prediction[0])})

    except Exception as e:
        return jsonify({"error": str(e)}), 500


if __name__ == "__main__":
    app.run(debug=True, host="127.0.0.1", port=5000)
