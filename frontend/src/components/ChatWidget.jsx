import React, { useState } from "react";

const ChatWidget = () => {
  const [open, setOpen] = useState(false);
  const [messages, setMessages] = useState([
    { sender: "ai", text: "Hello 👋 How can I help you?" },
  ]);
  const [input, setInput] = useState("");

  const sendMessage = () => {
    if (!input) return;

    const newMessages = [
      ...messages,
      { sender: "user", text: input },
      { sender: "ai", text: "Mock AI response..." },
    ];

    setMessages(newMessages);
    setInput("");
  };

  return (
    <>
      <button
        onClick={() => setOpen(!open)}
        className="fixed bottom-6 right-6 bg-indigo-600 text-white w-14 h-14 rounded-full shadow-lg"
      >
        💬
      </button>

      {open && (
        <div className="fixed bottom-24 right-6 w-80 bg-white shadow-xl rounded-2xl flex flex-col">
          <div className="p-4 border-b font-semibold text-indigo-600">
            AI Assistant
          </div>

          <div className="p-4 flex-1 overflow-y-auto space-y-2">
            {messages.map((msg, index) => (
              <div
                key={index}
                className={`p-2 rounded-lg text-sm ${
                  msg.sender === "user"
                    ? "bg-indigo-100 self-end"
                    : "bg-gray-100"
                }`}
              >
                {msg.text}
              </div>
            ))}
          </div>

          <div className="p-3 flex gap-2 border-t">
            <input
              type="text"
              className="flex-1 border rounded-lg px-2 py-1 text-sm"
              value={input}
              onChange={(e) => setInput(e.target.value)}
            />
            <button onClick={sendMessage} className="btn-primary text-sm">
              Send
            </button>
          </div>
        </div>
      )}
    </>
  );
};

export default ChatWidget;
