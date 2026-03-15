import React, { useState } from "react";
import { askAssistant } from "../../sevice/providerService";

const AIAssistant = () => {
  const [messages, setMessages] = useState([
    {
      sender: "ai",
      text: "Hello 👋 I'm your marketplace assistant. How can I help?",
    },
  ]);

  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);

  const sendMessage = async () => {
    if (!input.trim()) return;

    const userMessage = { sender: "user", text: input };
    setMessages((prev) => [...prev, userMessage]);
    setInput("");
    setLoading(true);
    try {
      const data = await askAssistant(userMessage.text);

      const aiMessage = {
        sender: "ai",
        text: data.message,
      };

      setMessages((prev) => [...prev, aiMessage]);
    } catch (err) {
      console.error("AI request failed:", err);
      setMessages((prev) => [
        ...prev,
        { sender: "ai", text: "Sorry, something went wrong." },
      ]);
    }

    setLoading(false);
  };

  return (
    <div className="card flex flex-col h-[75vh]">
      {/* Header */}
      <div className="border-b pb-3 mb-4">
        <h2 className="text-xl font-bold text-indigo-600">
          AI Marketplace Assistant
        </h2>
        <p className="text-sm text-gray-500">
          Ask about providers, jobs, recommendations, or wallet details.
        </p>
      </div>

      {/* Chat Messages */}
      <div className="flex-1 overflow-y-auto space-y-3 pr-2">
        {messages.map((msg, index) => (
          <div
            key={index}
            className={`max-w-[70%] px-4 py-2 rounded-xl text-sm ${
              msg.sender === "user"
                ? "ml-auto bg-indigo-600 text-white"
                : "bg-gray-100"
            }`}
          >
            {msg.text}
          </div>
        ))}
      </div>

      {/* Input */}
      <div className="mt-4 flex gap-3">
        <input
          type="text"
          className="flex-1 border rounded-xl px-3 py-2 text-sm"
          placeholder="Type your message..."
          value={input}
          onChange={(e) => setInput(e.target.value)}
        />
        <button onClick={sendMessage} className="btn-primary">
          Send
        </button>
      </div>
    </div>
  );
};

export default AIAssistant;
