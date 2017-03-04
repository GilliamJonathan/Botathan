{\rtf1\ansi\ansicpg1252\cocoartf1504\cocoasubrtf810
{\fonttbl\f0\fswiss\fcharset0 Helvetica;}
{\colortbl;\red255\green255\blue255;}
{\*\expandedcolortbl;;}
\margl1440\margr1440\vieww28600\viewh18000\viewkind0
\pard\tx720\tx1440\tx2160\tx2880\tx3600\tx4320\tx5040\tx5760\tx6480\tx7200\tx7920\tx8640\pardirnatural\partightenfactor0

\f0\fs24 \cf0 public class ChattyModule\
\{\
	_client.MessageReceived += async (s, e) =>\
	\{\
           	if (!e.Message.IsAuthor)\
           	\{\
			if (e.Message.RawText.ToUpper().Replace(\'93 \'93,\'94\'94).Contains("RIPBOTATHAN"))\
				await e.Channel.SendMessage("Stop making fun of me, I'm doing the best I can :cry:");\
			if (e.Message.RawText.ToUpper().Replace(\'93 \'93,\'94\'94).Contains(\'93BOTATHAN2.0\'94))\
\pard\tx720\tx1440\tx2160\tx2880\tx3600\tx4320\tx5040\tx5760\tx6480\tx7200\tx7920\tx8640\pardirnatural\partightenfactor0
\cf0 				await e.Channel.SendMessage(\'93why are you talking about _him_\'93);\
			if (e.Message.RawText.ToUpper().Replace(\'93 \'93,\'94\'94).Contains(\'93ILIKECAT\'94))\
				await e.Channel.SendMessage(\'93Meow\'94);\
			if (e.Message.RawText.ToUpper().Replace(\'93 \'93,\'94\'94).Contains(\'93BOTATHANSUCKS\'94))\
				await e.Channel.SendMessage(\'93You wish I _sucked_\'93);\
\pard\tx720\tx1440\tx2160\tx2880\tx3600\tx4320\tx5040\tx5760\tx6480\tx7200\tx7920\tx8640\pardirnatural\partightenfactor0
\cf0 		\}\
	\};\
\}}