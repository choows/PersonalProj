import pandas as pd
from nltk.corpus import wordnet as wn
import requests
from bs4 import BeautifulSoup

class inptClass:
    def GetPositiveWord(self):
        return pd.read_excel(r'LoughranMcDonald_SentimentWordLists_2018.xlsx', sheet_name='Positive').values.tolist()
    
    def GetNegativeWord(self):
        return pd.read_excel(r'LoughranMcDonald_SentimentWordLists_2018.xlsx', sheet_name='Negative').values.tolist()

    def GetSpecWord(self):
        return pd.read_excel(r'LoughranMcDonald_SentimentWordLists_2018.xlsx', sheet_name='Uncertainty').values.tolist()

    def GetNouncesWord(self):
        return {x.name().split('.', 1)[0] for x in wn.all_synsets('n')}
    
    def GetRequest(self):
        resp = []
        text_file = open("SampleInput.txt", encoding="utf8")
        content = text_file.read().split(" ")
        text_file2 =  open("SampleInput2.txt", encoding="utf8").read().split(" ")
        text_file3 =  open("SampleInput3.txt", encoding="utf8").read().split(" ")
        text_file4 =  open("SampleInput4.txt", encoding="utf8").read().split(" ")
        text_file5 =  open("SampleInput5.txt", encoding="utf8").read().split(" ")
        resp.append(content)
        resp.append(text_file2)
        resp.append(text_file3)
        resp.append(text_file4)
        resp.append(text_file5)
        resp.append(open("SampleInput6.txt", encoding="utf8").read().split(" "))
        resp.append(open("SampleInput7.txt", encoding="utf8").read().split(" "))
        resp.append(open("SampleInput8.txt", encoding="utf8").read().split(" "))
        resp.append(open("SampleInput9.txt", encoding="utf8").read().split(" "))
        resp.append(open("SampleInput10.txt", encoding="utf8").read().split(" "))
        return resp