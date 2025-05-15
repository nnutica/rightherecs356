from fastapi import FastAPI
from transformers import AutoConfig, AutoModelForSequenceClassification, AutoTokenizer
import numpy as np
from google import genai
import torch
import os
from dotenv import load_dotenv
from pydantic import BaseModel
import uvicorn 

# โหลด environment variables
load_dotenv()

# โหลด API Key จาก .env
API_KEY = os.getenv("apikey")
HUGGINGFACE_TOKEN = os.getenv("HuggingFaceToken")

# ตรวจสอบว่า API Key โหลดถูกต้อง
if not API_KEY:
    raise ValueError("Google Gemini API Key ไม่พบใน .env")

client = genai.Client(api_key=API_KEY)

# โหลดโมเดลจาก Hugging Face Model Hub
MODEL_NAME = "Gun555/Righthere"

# Map ตัวเลขเป็นอารมณ์
label_map = {
    0: "sadness",
    1: "joy",
    2: "love",
    3: "anger",
    4: "fear",
    5: "surprise"
}

# โหลดโมเดลและ Tokenizer
config = AutoConfig.from_pretrained(MODEL_NAME, token=HUGGINGFACE_TOKEN)
model = AutoModelForSequenceClassification.from_pretrained(
    MODEL_NAME, config=config, torch_dtype=torch.float16, token=HUGGINGFACE_TOKEN
)
tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME, token=HUGGINGFACE_TOKEN)

# สร้าง FastAPI instance
app = FastAPI()

# สร้างโครงสร้างรับข้อมูล
class Advice(BaseModel):
    text: str

@app.post("/getadvice")
async def get_advice(data: Advice):
    diary_text = data.text

    # Tokenize ข้อความ
    encoding = tokenizer(
        diary_text, truncation=True, padding="max_length", max_length=128, return_tensors="pt"
    )

    # ทำนายอารมณ์
    output = model(**encoding)
    logits = output.logits.detach().cpu().numpy()
    predicted_index = int(np.argmax(logits, axis=-1))
    predicted_emotion = label_map.get(predicted_index, "Unknown")

    # สร้าง Prompt สำหรับ AI
    prompt = f"""
        ### Role: System (AI Therapist)
        You are an empathetic AI therapist trained to analyze users' diary entries. Your goal is to help users understand their emotions through diary analysis and provide compassionate feedback. Your analysis should be based on the provided diary entry and the pre-detected emotion.

        ### Role: User (Diary Writer)
        Diary Entry:
        "{diary_text}"

        ### Pre-Detected Emotion (from specialized model):
        {predicted_emotion}

        ### Task:
        - **Analyze** the user's overall emotional state based on the diary entry and the pre-detected emotion.
        - **Provide** a structured response including:
        1. **Suggestion**: A supportive and positive message and get advice. Your advice should be actionable and tailored to the user's situation as described in the diary and their pre-detected emotion.
        2. **Emotional Reflection**: Summarize the user's emotions and the situation described to help them understand their feelings and experiences.
        3. **Mood**: Assign a one-word general emotional label that best describes the overall tone of the diary entry. This might be similar to or broader than the pre-detected emotion.
        4. **Keyword Extraction**: Identify 3-5 key topics or themes from the diary entry.
        5. **Sentiment Score**: Assign a numerical sentiment score to the diary entry, ranging from -1.0 (very negative) to +1.0 (very positive), with 0.0 representing a neutral sentiment. This score should reflect the overall emotional valence of the entry.

    ### Response Format (Strictly follow this format):
    - Suggestion: <Your response>
    - Emotional Reflection: <Your response>
    - Mood: <Your response>
    - Keywords: <Keyword1, Keyword2, Keyword3, ...>
    - Sentiment Score: <Numerical value between -1.0 and +1.0, e.g., 0.75 or -0.5>
    """

    # เรียกใช้ Gemini API
    response = client.models.generate_content(
        model="gemini-2.0-flash",
        contents=prompt
    )

    return {"emotion": predicted_emotion, "advice": response.text}

@app.get("/")
async def home():
    return {"message": "Welcome to AI API"}


uvicorn.run(app, host="127.0.0.1", port=int(os.getenv("PORT", "8000")))