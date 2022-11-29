import pickle
import sys

loaded_model = pickle.load(open(sys.argv[1], 'rb'))
print(loaded_model.predict([sys.argv[2].split(',')])[0])