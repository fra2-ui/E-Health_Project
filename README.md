# E-Health Project: Psychometric Personas & Serious Game (2025/2026)

[![Unity Version](https://img.shields.io/badge/Unity-6000.2.7f2%20(Unity%206)-blue.svg?logo=unity)](https://unity.com/)
[![Python Version](https://img.shields.io/badge/Python-3.11.14-blue.svg?logo=python)](https://www.python.org/)
[![Render Pipeline](https://img.shields.io/badge/Pipeline-URP%202D-green.svg)](https://unity.com/features/universal-render-pipeline)
[![Domain](https://img.shields.io/badge/Domain-E--Health%20%7C%20Serious%20Games-orange.svg)]()

An interdisciplinary **E-Health Serious Game** and **Computational Psychometrics** project that bridges advanced unsupervised machine learning with interactive game design. 

By analyzing empirical questionnaire data through statistical modeling and clustering techniques, this project derives four scientifically grounded **psychological archetypes (Personas)** and embodies them into an immersive 2D narrative game. In-game mechanics directly simulate mental health symptoms—such as trauma flashbacks, substance intoxication, cognitive overload, and digital dissociation—across a 7-day story cycle.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Repository Structure](#repository-structure)
- [Part 1: Psychometrics & Machine Learning Pipeline (`Personas/`)](#part-1-psychometrics--machine-learning-pipeline-personas)
  - [Validated Instruments](#validated-instruments)
  - [Analytical Workflow](#analytical-workflow)
  - [The 4 Psychological Personas](#the-4-psychological-personas)
- [Part 2: The Serious Game (`EHealth_Project/`)](#part-2-the-serious-game-ehealth_project)
  - [Game Design & Core Loop](#game-design--core-loop)
  - [Symptom Simulation Mechanics](#symptom-simulation-mechanics)
  - [7-Day Narrative Structure & Minigames](#7-day-narrative-structure--minigames)
  - [Scenes & Town Exploration](#scenes--town-exploration)
- [Getting Started & Installation](#getting-started--installation)
  - [Python Environment (Data Science Pipeline)](#python-environment-data-science-pipeline)
  - [Unity Project (Game Client)](#unity-project-game-client)
- [Technology Stack](#technology-stack)
- [Academic Context](#academic-context)

---

## Project Overview

Traditional serious games often rely on generalized or stereotypical depictions of mental health conditions. This project introduces a data-driven methodology:
1. **Empirical Grounding**: Collection of psychometric data evaluating trauma, substance abuse, digital dependency, social support, and subjective well-being.
2. **Unsupervised Clustering Benchmark**: Rigorous model selection comparing multiple distance metrics and clustering algorithms to identify mathematically separate and clinically significant population archetypes.
3. **Translational Game Design**: Mapping quantitative behavioral profiles into playable personas, expressive mechanics (URP post-processing, input delay, control inversion), environmental storytelling, and thematic minigames.

---

## Repository Structure

```plaintext
.
├── Personas/                                # Data Science & Psychometric Analysis
│   ├── Papers/                              # Scientific literature for validated scales (AUDIT, PCL-5, IAT, etc.)
│   ├── Questionnaire/                       # Survey codebooks and metadata (.xlsx)
│   ├── Python/                              # Statistical scripts, notebooks, datasets & results
│   │   ├── dataset_project_eHealth20252026.csv   # Raw / Preprocessed dataset
│   │   ├── eHealthProject_Notebook.ipynb    # Full Jupyter analysis notebook
│   │   ├── eHealthProject_Notebook.py       # Executable Python script version
│   │   ├── requirements.txt                 # Exact Python dependencies
│   │   └── cluster_persona_profiles.xlsx    # Exported statistical persona profiles
│   └── README.txt                           # Overview of the clustering pipeline
│
└── EHealth_Project/                         # Unity 6 Serious Game Project
    ├── Assets/
    │   ├── _GAME_/                          # Primary gameplay assets & systems
    │   │   ├── Core/                        # URP 2D renderer settings & globals
    │   │   ├── Player/                      # Player controller, input actions, animators
    │   │   ├── Prefabs/                     # Player variants (Alex, Anna, Ardley, Colt), NPCs, UI
    │   │   ├── Scenes/                      # MainMenu, OutWorld, Credits, Final, and Interiors
    │   │   │   └── Interni/                 # Buildings: Arcade, Bakery, Cinema, Pub, Library, etc.
    │   │   ├── Story/                       # 7-Day ScriptableObject quest database & missions
    │   │   ├── UI Management/               # UIManager, audio, wellbeing HUD
    │   │   └── World/                       # Town map logic, spawn points, faders
    │   ├── MiniGames/                       # Thematic mini-games
    │   │   ├── EndlessRun/                  # Endurance & burnout runner minigame
    │   │   ├── FallingObject/               # Focus & reflex object-catching minigame
    │   │   ├── FirstNote/                   # Guitar tuning audio minigame
    │   │   └── TastiAComparsa/              # Quick-Time-Event (QTE) reaction minigame
    │   ├── UI/                              # Character selection and loading screens
    │   └── mappa_gioco.tmx                  # Tiled world map imported via SuperTiled2Unity
    ├── Packages/                            # Unity package manifest & locks
    └── ProjectSettings/                     # Project configuration & build settings
```

---

## Part 1: Psychometrics & Machine Learning Pipeline (`Personas/`)

### Validated Instruments

The dataset integrates standardized psychometric batteries:
* **AUDIT** (*Alcohol Use Disorders Identification Test*): Identifies hazardous and harmful alcohol consumption patterns.
* **DAST-10** (*Drug Abuse Screening Test*): Quantifies consequences related to drug abuse.
* **IAT** (*Internet Addiction Test*): Measures the severity of compulsive internet and digital overuse.
* **PCL-5** (*PTSD Checklist for DSM-5*): Assesses post-traumatic stress disorder symptoms.
* **PGSI** (*Problem Gambling Severity Index*): Evaluates gambling behavior and associated harm.
* **MSPSS** (*Multidimensional Scale of Perceived Social Support*): Assesses perceived support from family, friends, and significant others.
* **WHO-5** (*WHO-Five Well-Being Index*): Gauges subjective psychological well-being.
* **SWLS** (*Satisfaction with Life Scale*): Measures global cognitive judgments of life satisfaction.

### Analytical Workflow

The pipeline implemented in [`Personas/Python/eHealthProject_Notebook.ipynb`](Personas/Python/eHealthProject_Notebook.ipynb) executes:
1. **Data Cleaning & Harmonization**: Column name normalization, noise handling, removal of non-informative answers, and raw structural correlation checks.
2. **Hybrid Imputation**:
   - Random Forest imputation for missing gender values.
   - Median/Mode imputation for socioeconomic and demographic indicators.
   - Row-wise mean imputation for psychometric scale items (applied when $\ge 60\%$ of items are present).
3. **Exploratory Data Analysis (EDA)**:
   - Normality diagnostics (Lilliefors / Kolmogorov-Smirnov tests) to inform parametric vs. non-parametric testing.
   - Correlation heatmaps and multivariate linear regressions (e.g., verifying the strong relationship between trauma and substance dependency, $R^2=0.88$).
4. **Clustering Benchmarking**:
   - Compares **K-Medoids (PAM)** and **Hierarchical Agglomerative Clustering** across four representations:
     - **Z-Score Standardization** (Standard baseline)
     - **Gower Distance** (Handles mixed continuous and categorical types)
     - **FAMD** (*Factor Analysis of Mixed Data*)
     - **PCA** (*Principal Component Analysis* on numerical psychometrics)
   - Evaluated via a multi-objective Quality Score combining **Silhouette Width** (geometric cohesion) and **% Statistical Difference** (clinical/discriminative validity), cross-referenced with Inertia Elbow analysis.
5. **Statistical Profiling & Verification**:
   - Global hypothesis testing (ANOVA / Kruskal-Wallis for numeric distributions, Chi-Square / Fisher’s Exact for categorical distributions).
   - Pairwise post-hoc tests with **Bonferroni corrections** to verify pairwise boundaries between clusters.
   - 3D PCA projection verifying spatial separation across principal axes.

### The 4 Psychological Personas

The clustering model converged on a 4-cluster solution ($92.3\%$ statistical divergence across psychometric variables), yielding four actionable archetypes:

| Persona | Archetype Name | Key Demographic Profile | Dominant Psychological Profile | Primary Coping Mechanism |
| :--- | :--- | :--- | :--- | :--- |
| **Alex** | *The Vulnerable Drinker* | 28 y/o, married, elementary education, low income (~€19,000) | Highest Alcohol Risk (AUDIT ~25.5), elevated gambling risk (PGSI), low digital use | Real-world distractions, alcohol, and gambling under financial stress |
| **Colt** | *The Traumatized & High-Risk* | 28 y/o, married, high school diploma, moderate income (~€33,000) | Severe PTSD / Stress (PCL-5 ~60), high drug abuse (DAST-10), extreme isolation | Hard drugs and heavy digital dissociation to numb traumatic memories |
| **Ardley** | *The Tech-Addicted Loner* | 28 y/o, single, freelancer, low income (~€17,500) | Severe Internet Addiction (IAT > 90), zero substance abuse, low physical support | Digital escapism; relies on virtual communities as a substitute for real-world support |
| **Anna** | *The Resilient High-Achiever* | 29 y/o, married manager, Master's degree, high income (~€59,000) | Highest Well-Being (WHO-5 / SWLS), trauma-free, low-risk substance profile | Occasional/recreational consumption; resilient against economic stressors |

---

## Part 2: The Serious Game (`EHealth_Project/`)

The serious game is an interactive 2D RPG built with **Unity 6 (6000.2.7f2)** and the **Universal Render Pipeline (URP 2D)**.

```
       [Main Menu] 
            │
    [Character Select] ──► Choose Persona (Alex / Colt / Ardley / Anna)
            │
     [Loading Screen]
            │
       [TownScene] ──────► Talk to NPCs / Manage Wellbeing / Experience Triggers
            │
    [Daily Quest / Day Cycle]
            │
        ┌───┴─────────────────────────────┐
     Day 2 / 4 / 6 Minigames            Day 1 / 3 / 5 / 7 Narrative
  (FirstNote, FallingObject,            (Dialogues, Environmental
       EndlessRun)                       Storytelling, Finale)
            │
    [Return to Town with Wellbeing Delta & Progression]
```

### Game Design & Core Loop
* **Character Selection**: Players select one of the four personas, each initialized with unique character traits, animated sprites, and psychological conditions.
* **Wellbeing System**: A continuous health/wellbeing meter managed by [`StatusManager.cs`](EHealth_Project/Assets/_GAME_/Prefabs/NPC/Distractors/Scripts/StatusManager.cs). Positive coping activities and successful minigame completion restore wellbeing; stress triggers and substance use degrade it.
* **Persistent Game State**: `GameManager` and `QuestManager` persist across scene transitions using `DontDestroyOnLoad`.

### Symptom Simulation Mechanics

The game uses sensory and input alterations to convey mental health states:
* **Alcohol Intoxication Post-FX**: Dynamic URP Volume fading controlled via `StatusManager.TriggerAlcoholEffect()`. Blur, vignette, and distortion simulate impairment.
* **Trauma Lag & Flashback Stutter**: In `StatusManager.TriggerTraumaLag()`, sudden lag spikes and temporal position resets simulate PTSD dissociation and memory freezing.
* **Control Inversion**: Simulates cognitive disorientation and loss of motor control under acute crisis.
* **Social Notification Distractors**: Popups and sound alerts simulating smartphone hyper-connectivity, intrusive thoughts, and digital compulsion ([`SocialNotificationsController.cs`](EHealth_Project/Assets/_GAME_/Prefabs/NPC/Distractors/Scripts/SocialNotificationsController.cs)).

### 7-Day Narrative Structure & Minigames

The narrative spans seven structured days loaded dynamically from ScriptableObjects:
1. **Day 1: The Static Town**: Narrative setup, establishing routine, exploring town surroundings and NPC interactions.
2. **Day 2: The Calibration**: Introduction of coping mechanisms. Concludes with the **FirstNote** minigame (guitar tuning, focus, auditory grounding).
3. **Day 3: The Phantom Signal**: Escalating stressors, social pressure, and dialogue dilemmas.
4. **Day 4: The Removal**: Facing mounting life pressures. Concludes with the **FallingObject** minigame (stress response, reflex catching, obstacle avoidance).
5. **Day 5: The Glitch**: Cognitive dissonance, sensory symptoms, and breakdown of everyday certainties.
6. **Day 6: The Collapse**: Climax of psychological crisis. Concludes with the **EndlessRun** minigame (endurance, burnout, and escape).
7. **Day 7: The Awakening**: Resolution, reflection, and outcome determined by the player’s journey and character archetype.

### Scenes & Town Exploration
* **Main Hub**: `OutWorld` / `TownScene` featuring a full 2D tilemap imported via **SuperTiled2Unity** ([`mappa_gioco.tmx`](EHealth_Project/Assets/mappa_gioco.tmx)).
* **Interactive Interiors**:
  * `int_ARCADE` (Gambling and escapism triggers)
  * `interni_PUB` (Alcohol and social vices)
  * `Int_COMMUNITY_CENTER` (Support and social connection)
  * `LIBRARY` (Reflection and solitude)
  * `int_BAKERY`, `Int_SUPERMARKET`, `int_SCHOOL`, `int_CINEMA`

---

## Getting Started & Installation

### Python Environment (Data Science Pipeline)

> [!IMPORTANT]
> The statistical clustering benchmark and analysis scripts are validated on **Python 3.11.14**. Using different minor versions may cause differences in package resolution or random-seed clustering behavior.

1. Navigate to the Python directory:
   ```bash
   cd Personas/Python
   ```

2. Create and activate a virtual environment:
   ```bash
   python3.11 -m venv .venv
   source .venv/bin/activate    # On Windows: .venv\Scripts\activate
   ```

3. Install required dependencies:
   ```bash
   pip install --upgrade pip
   pip install -r requirements.txt
   ```

4. Run the Jupyter Notebook or standalone script:
   ```bash
   # Launch interactive notebook
   jupyter lab eHealthProject_Notebook.ipynb

   # Or execute the Python script directly
   python eHealthProject_Notebook.py
   ```

---

### Unity Project (Game Client)

1. **Unity Version**: Install **Unity 6 (6000.2.7f2)** through the [Unity Hub](https://unity.com/download).
2. **Open Project**:
   - In Unity Hub, click **Add** $\rightarrow$ **Add project from disk**.
   - Select the `EHealth_Project/` folder.
   - Ensure the selected Editor Version is `6000.2.7f2`.
3. **Packages & Dependencies**:
   - Unity will automatically resolve packages defined in [`Packages/manifest.json`](EHealth_Project/Packages/manifest.json), including *Universal RP (17.2.0)*, *Input System (1.14.2)*, *Cinemachine (3.1.5)*, and *2D Tilemap Extras*.
4. **Running the Game**:
   - In the Project window, navigate to `Assets/_GAME_/Scenes/`.
   - Open **`MainMenu.unity`**.
   - Press **Play** in the Unity Editor toolbar.
   - Choose a Persona on the **Character Select** screen and begin the 7-day story.

---

## Technology Stack

### Data Science & Machine Learning
* **Language & Runtime**: Python 3.11.14
* **Core Libraries**: `numpy`, `pandas`, `scipy`, `statsmodels`
* **Machine Learning**: `scikit-learn`, `scikit-learn-extra` (PAM K-Medoids), `gower` (Gower distance), `prince` (FAMD)
* **Visualization & Reporting**: `matplotlib`, `seaborn`, `openpyxl`

### Game Engine & Development
* **Engine**: Unity 6 (`6000.2.7f2`)
* **Graphics Pipeline**: Universal Render Pipeline 2D (URP 2D)
* **Input**: Unity New Input System (`com.unity.inputsystem`)
* **Tilemap Interop**: SuperTiled2Unity & Tiled (`.tmx`)
* **Camera**: Unity Cinemachine 3
* **Audio**: Unity AudioSource / MusicManager system

---

## Academic Context

Developed as an **E-Health Project (Academic Year 2025/2026)** to investigate how computational data science and serious game design can synergistically model, explain, and foster empathy for mental health conditions.

