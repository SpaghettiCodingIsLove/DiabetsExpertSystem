import pickle
import sys
from sklearn import model_selection
from sklearn.neighbors import KNeighborsClassifier
from sklearn.pipeline import make_pipeline
from sklearn.preprocessing import StandardScaler

loaded_model = pickle.load(open(sys.argv[1], 'rb'))
print(loaded_model.predict([sys.argv[2].split(',')])[0])